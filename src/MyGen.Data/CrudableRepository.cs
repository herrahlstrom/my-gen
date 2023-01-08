using System.Collections.Concurrent;

namespace MyGen.Data;

public class CrudableRepository
{
   private readonly ConcurrentBag<Guid> _deletedItems = new();
   private readonly ConcurrentDictionary<Guid, ICrudable> _entities = new();
   private readonly IFileSystem _fileSystem;
   private readonly MyGenSerializer _serializer;
   private readonly ConcurrentDictionary<Guid, int> _tracker = new();

   public CrudableRepository(IFileSystem fileSystem)
   {
      _serializer = new MyGenSerializer();
      _fileSystem = fileSystem;
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
      var files = _fileSystem.GetFiles("*.json");
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

   private static string GetEntityName(ICrudable entity) => $"{entity.GetType().Name}-{entity.Id}.json";

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
      var entity = _serializer.Read(stream, out _);

      _entities.TryAdd(entity.Id, entity);
      _tracker.TryAdd(entity.Id, entity.GetHashCode());
   }

   private void SaveEntity<T>(T entity) where T : ICrudable
   {
      var state = GetState(entity);
      switch (state)
      {
         case EntityState.Added:
            using (var stream = _fileSystem.CreateFileStream(GetEntityName(entity)))
            {
               _serializer.Write(stream, entity);
               _tracker.TryAdd(entity.Id, entity.GetHashCode());
            }
            break;

         case EntityState.Deleted:
            _fileSystem.DeleteFile(GetEntityName(entity));
            _entities.Remove(entity.Id, out _);
            _tracker.Remove(entity.Id, out _);
            break;

         case EntityState.Modified:
            using (var stream = _fileSystem.CreateFileStream(GetEntityName(entity)))
            {
               _serializer.Write(stream, entity);
               _tracker[entity.Id] = entity.GetHashCode();
            }
            break;

         case EntityState.Unmodified:
            return;

         default:
            throw new NotSupportedException($"Invalid state of entity: {state}");
      }
   }
}