using MyGen.Data;

namespace MyGen.Model;

internal interface IEntityProvider<T>
{
   public T Get(Guid id);
}

internal abstract class EntityProvider
{
   protected EntityProvider(IEntityRepository entityRepository, EntityProviderFactory entityProviderFactory)
   {
      EntityRepository = entityRepository;
      EntityProviderFactory = entityProviderFactory;
   }

   protected EntityProviderFactory EntityProviderFactory { get; }
   protected IEntityRepository EntityRepository { get; }
}
