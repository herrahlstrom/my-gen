using MyGen.Data;
using MyGen.Data.Entities;

namespace MyGen.Model;

internal class FamilyProvider : EntityProvider, IEntityProvider<FamilyModel>
{
   public FamilyProvider(IEntityRepository entityRepository, EntityProviderFactory entityProviderFactory) : base(entityRepository, entityProviderFactory)
   {
   }

   public FamilyModel Get(Guid id)
   {
      return new FamilyModel(
         EntityRepository.GetEntity<Family>(id),
         EntityProviderFactory.GetPersonProvider(),
         EntityProviderFactory.GetLifeStoryProvider());
   }
}