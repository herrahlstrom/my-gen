using MyGen.Data.Models;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace MyGen.Data;

public class CrudableRepository
{
   private readonly ConcurrentBag<Guid> _deletedItems = new();
   private readonly ConcurrentDictionary<Guid, ICrudable> _entities = new();
   private readonly JsonSerializerOptions _jsonSerializerOptions;
   private readonly string _path;
   private readonly ConcurrentDictionary<Guid, int> _tracker = new();

   public CrudableRepository(string path)
   {
      _path = path;

      _jsonSerializerOptions = new JsonSerializerOptions
      {
         WriteIndented = true,
         DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
      };
   }

   private enum EntityState
   {
      Added,
      Modified,
      Unmodified,
      Deleted
   }

   public void AddEntity<T>(T entity) where T : ICrudable
   {
      _entities.TryAdd(entity.Id, entity);
   }

   public IEnumerable<T> GetEntities<T>() where T : ICrudable
   {
      return _entities.Values.OfType<T>();
   }

   public T GetEntity<T>(Guid id) where T : ICrudable
   {
      if (_entities.TryGetValue(id, out var entity) && entity is T item)
      {
         return item;
      }

      throw new ArgumentException($"Can't find person with id {id}");
   }

   public void Load()
   {
      var files = Directory.GetFiles(_path, "*.json");
      Parallel.ForEach(files, new ParallelOptions()
      {
         MaxDegreeOfParallelism = 7
      }, LoadFromFile);
   }

   public void RemoveEntity<T>(T entity) where T : ICrudable
   {
      _deletedItems.Add(entity.Id);
   }

   public void Save()
   {
      Parallel.ForEach(_entities.Values, new ParallelOptions()
      {
         MaxDegreeOfParallelism = 7
      }, SaveEntity);
   }

   private string GetEntityPath(ICrudable entity)
   {
      return Path.Combine(_path, $"{entity.GetType().Name}-{entity.Id}.json");
   }

   private EntityState GetState<T>(T entity) where T : ICrudable
   {
      if (_deletedItems.Contains(entity.Id))
      {
         return EntityState.Deleted;
      }

      if (_tracker.TryGetValue(entity.Id, out var mtag))
      {
         var hashCode = entity.GetHashCode();
         return hashCode == mtag
            ? EntityState.Unmodified
            : EntityState.Modified;
      }

      return EntityState.Added;
   }

   private void LoadFromFile(string path)
   {
      using var stream = File.OpenRead(path);
      using var reader = new StreamReader(stream, Encoding.UTF8);

      Dictionary<string, string> meta = new();
      while (true)
      {
         var line = reader.ReadLine();
         if (string.IsNullOrEmpty(line)) { break; }
         var p = line.IndexOf(':');
         meta.Add(line[..p], line[(p + 1)..]);
      }

      string type = meta["type"];
      int version = int.Parse(meta["version"]);

      ICrudable entity = type switch
      {
         nameof(Person) => JsonSerializer.Deserialize<Person>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Family) => JsonSerializer.Deserialize<Family>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Source) => JsonSerializer.Deserialize<Source>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(LifeStory) => JsonSerializer.Deserialize<LifeStory>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Media) => JsonSerializer.Deserialize<Media>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         _ => throw new InvalidDataException($"Can't deserialize type {type}")
      };

      _entities.TryAdd(entity.Id, entity);
      _tracker.TryAdd(entity.Id, entity.GetHashCode());
   }

   private void SaveEntity<T>(T entity) where T : ICrudable
   {
      var state = GetState(entity);
      switch (state)
      {
         case EntityState.Added:
            WriteContent(entity, FileMode.CreateNew);
            _tracker.TryAdd(entity.Id, entity.GetHashCode());
            break;

         case EntityState.Deleted:
            File.Delete(GetEntityPath(entity));
            _entities.Remove(entity.Id, out _);
            _tracker.Remove(entity.Id, out _);
            break;

         case EntityState.Modified:
            WriteContent(entity, FileMode.Create);
            _tracker[entity.Id] = entity.GetHashCode();
            break;

         case EntityState.Unmodified:
            return;

         default:
            throw new NotSupportedException($"Invalid state of entity: {state}");
      }
   }

   private void WriteContent<T>(T entity, FileMode fileMode) where T : ICrudable
   {
      using var stream = new FileStream(GetEntityPath(entity), fileMode);
      using var writer = new StreamWriter(stream, Encoding.UTF8);
      writer.WriteLine("type:{0}", entity.GetType().Name);
      writer.WriteLine("version:{0}", entity.Version);
      writer.WriteLine("updated:{0:o}", DateTime.UtcNow);
      writer.WriteLine();

      var json = JsonSerializer.Serialize(entity, entity.GetType(), _jsonSerializerOptions);
      writer.WriteLine(json);
   }
}