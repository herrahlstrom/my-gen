using MyGen.Data;

namespace MyGen.Model;

public interface IModelRepository
{
   public PersonModel GetPerson(Guid id);

   public IEnumerable<PersonModel> GetPersons();

   void Save();
}

public class ModelRepository : IModelRepository
{
   private readonly IEntityRepository _entityRepository;

   public ModelRepository(IEntityRepository entityRepository)
   {
      _entityRepository = entityRepository;

      EntityProviderFactory = new(entityRepository);
   }

   internal EntityProviderFactory EntityProviderFactory { get; }

   public PersonModel GetPerson(Guid id)
   {
      return EntityProviderFactory.GetPersonProvider().Get(id);
   }

   public IEnumerable<PersonModel> GetPersons()
   {
      return EntityProviderFactory.GetPersonProvider().GetAll();
   }

   public void Save()
   {
      _entityRepository.Save();
   }
}