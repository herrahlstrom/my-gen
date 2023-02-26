using MyGen.Model;
using MyGen.Shared;
using MyGen.Wpf.Shared;
using System.Linq;

namespace MyGen.Wpf.Person;

internal class PersonViewModelRepository : IViewModelRepository<PersonViewModel>
{
   private readonly IModelRepository _modelRepository;

   public PersonViewModelRepository(IModelRepository modelRepository)
   {
      _modelRepository = modelRepository;
   }

   public void LoadModel(PersonViewModel viewModel)
   {
      var person = _modelRepository.GetPerson(viewModel.Id);

      viewModel.Name = person.Name;

      viewModel.Birth = new EventViewModel(
         person.Birth?.Date ?? DateModel.Empty,
         person.Birth?.Location ?? "");

      viewModel.Death = new EventViewModel(
         person.Death?.Date ?? DateModel.Empty,
         person.Death?.Location ?? "");

      viewModel.Sex = person.Sex;
      viewModel.Profession = person.Profession;
      viewModel.Notes = person.Notes;

      viewModel.Father = (from f in person.FamiliesAsChild
                          where f.Husband != null
                          select GetPersonSlim(f.Husband)).FirstOrDefault();

      viewModel.Mother = (from f in person.FamiliesAsChild
                          where f.Wife != null
                          select GetPersonSlim(f.Wife)).FirstOrDefault();
   }

   public void SaveModel(PersonViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }

   private static IPerson? GetPersonSlim(Model.PersonModel person)
   {
      return new SlimPersonViewModel()
      {
         Name = person.Name,
         BirthDate = person.Birth?.Date ?? DateModel.Empty,
         DeathDate = person.Death?.Date ?? DateModel.Empty
      };
   }
}