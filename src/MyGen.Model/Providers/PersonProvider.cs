using MyGen.Data;
using MyGen.Data.Entities;

namespace MyGen.Model;

internal class PersonProvider : EntityProvider, IEntityProvider<PersonModel>
{
   public PersonProvider(IEntityRepository entityRepository, EntityProviderFactory entityProviderFactory) : base(entityRepository, entityProviderFactory)
   {
   }

   public PersonModel Get(Guid id)
   {
      return new PersonModel(
         EntityRepository.GetEntity<Person>(id),
         EntityProviderFactory.GetLifeStoryProvider(),
         EntityProviderFactory.GetFamilyProvider());
   }

   public IEnumerable<PersonModel> GetAll()
   {
      var lifeStoryProvider = EntityProviderFactory.GetLifeStoryProvider();
      var familyProvider = EntityProviderFactory.GetFamilyProvider();
      foreach (var entity in EntityRepository.GetEntities<Person>())
      {
         yield return new PersonModel(entity, lifeStoryProvider, familyProvider);
      }
   }
}