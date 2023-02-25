using MyGen.Data.Entities;

namespace MyGen.Data;

public interface IEntityRepository
{
   void AddEntity<T>(T entity) where T : ICrudable;
   IEnumerable<T> GetEntities<T>() where T : ICrudable;
   T GetEntity<T>(Guid id) where T : ICrudable;
   void Load();
   void RemoveEntity<T>(T entity) where T : ICrudable;
   void Save();
}
