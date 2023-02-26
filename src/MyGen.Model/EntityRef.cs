namespace MyGen.Model;

public interface IEntityRef<T>
{
   T Entity { get; }
   Guid Id { get; }
}

public class EntityRef<T> : IEntityRef<T>
{
   private readonly Lazy<T> _childEntity;

   internal EntityRef(Guid id, IEntityProvider<T> provider)
   {
      Id = id;
      _childEntity = new Lazy<T>(() => provider.Get(Id));
   }

   public T Entity => _childEntity.Value;
   public Guid Id { get; }
}