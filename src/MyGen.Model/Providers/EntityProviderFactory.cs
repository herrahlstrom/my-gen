using MyGen.Data;

namespace MyGen.Model;

internal class EntityProviderFactory
{
   private readonly IEntityRepository _entityRepository;

   public EntityProviderFactory(IEntityRepository entityRepository)
   {
      _entityRepository = entityRepository;
   }

   public FamilyProvider GetFamilyProvider() => new FamilyProvider(_entityRepository, this);

   public LifeStoryProvider GetLifeStoryProvider() => new LifeStoryProvider(_entityRepository, this);

   public PersonProvider GetPersonProvider() => new PersonProvider(_entityRepository, this);
}