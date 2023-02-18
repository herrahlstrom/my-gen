using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MyGen.Data;

public class CrudableRepository
{
   private readonly ConcurrentBag<Guid> _deletedItems = new();
   private readonly ConcurrentDictionary<Guid, ICrudable> _entities = new();
   private readonly IFileSystem _fileSystem;
   private readonly ILogger<CrudableRepository> _logger;
   private readonly MyGenSerializer _serializer;
   private readonly ConcurrentDictionary<Guid, int> _tracker = new();
   public CrudableRepository(IFileSystem fileSystem, ILogger<CrudableRepository> logger)
   {
      _serializer = new MyGenSerializer();
      _fileSystem = fileSystem;
      _logger = logger;
   }

   private enum EntityState
   {
      Added,
      Modified,
      Unmodified,
      Deleted
   }

   public bool IsLoaded { get; private set; }
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
      if (IsLoaded)
      {
         _logger.LogTrace("Skip loading, already loaded.");
         return;
      }

      var timestamp = Stopwatch.GetTimestamp();

      var files = _fileSystem.GetFiles(".json");
      Parallel.ForEach(files, new ParallelOptions()
      {
         MaxDegreeOfParallelism = 7
      }, LoadFromFile);

      IsLoaded = true;

      var elapsed = Stopwatch.GetElapsedTime(timestamp);
      _logger.LogInformation("{0} files loaded in {1:N2} seconds", files.Count, elapsed.TotalSeconds);
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

   public bool TryGetEntity<T>(Guid id, [MaybeNullWhen(false)] out T entity) where T : ICrudable
   {
      if (_entities.TryGetValue(id, out var crudable) && crudable is T item)
      {
         entity = item;
         return true;
      }

      entity = default;
      return false;
   }

   internal static int GetHashCodeFromCollection<T>(IEnumerable<T>? collection) where T : notnull
   {
      if (collection is null)
      {
         return 0;
      }

      int hc = 0;
      foreach (var item in collection)
      {
         hc ^= item.GetHashCode();
         hc = (hc << 7) | (hc >> (32 - 7)); //rotale hc to the left to swipe over all bits
      }
      return hc;
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
      using var stream = _fileSystem.OpenForRead(path);
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
            using (var stream = _fileSystem.OpenForWrite(GetEntityName(entity)))
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
            using (var stream = _fileSystem.OpenForWrite(GetEntityName(entity)))
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