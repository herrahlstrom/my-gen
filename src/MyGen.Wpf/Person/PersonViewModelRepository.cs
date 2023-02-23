using MyGen.Data;
using MyGen.Data.Models;
using MyGen.Shared;
using MyGen.Shared.Extensions;
using MyGen.Wpf.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace MyGen.Wpf.Person;

internal class PersonViewModelRepository : IViewModelRepository<PersonViewModel>
{
   private readonly CrudableRepository _crudableRepository;

   public PersonViewModelRepository(CrudableRepository crudableRepository)
   {
      _crudableRepository = crudableRepository;
   }

   public async Task LoadModelAsync(PersonViewModel viewModel)
   {
      await Task.Run(_crudableRepository.Load);

      var person = _crudableRepository.GetPerson(viewModel.Id);

      viewModel.Name = new(person.Firstname, person.Lastname);
      viewModel.Sex = Mapper.ToSex(person.Sex);
      viewModel.Profession = person.Profession ?? "";
      viewModel.Notes = person.Notes ?? "";

      foreach (var lifeStoryMember in person.LifeStories)
      {
         var lifeStory = _crudableRepository.GetLifeStory(lifeStoryMember.LifeStoryId);

         switch (lifeStory.Type)
         {
            case LifeStoryType.Födelse:
               viewModel.Birth = CreateEventViewModel(lifeStoryMember, lifeStory);
               break;

            case LifeStoryType.Död:
               viewModel.Death = CreateEventViewModel(lifeStoryMember, lifeStory);
               break;
         }
      }

//      var famIds = person.Families.Select(x => x.FamilyId).ToList();
//      var relPersons = _crudableRepository.GetPersons().Where(x => x.Families.Any(y => famIds.Contains(y.FamilyId))).ToList();

      foreach (var familyMember in person.Families)
      {
         var family = _crudableRepository.GetFamily(familyMember.FamilyId);

         if (familyMember.MemberType.IsChild())
         {
            viewModel.Father = (from p in _crudableRepository.GetPersons()
                                from pFam in p.Families.Where(x => x.FamilyId == family.Id)
                                where pFam.MemberType == FamilyMemberType.Husband
                                select GetPersonSlim(p)).FirstOrDefault();
            
            viewModel.Mother = (from p in _crudableRepository.GetPersons()
                                from pFam in p.Families.Where(x => x.FamilyId == family.Id)
                                where pFam.MemberType == FamilyMemberType.Wife
                                select GetPersonSlim(p)).FirstOrDefault();
         }
      }
   }

   private IPerson? GetPersonSlim(Data.Models.Person person)
   {


      return new SlimPersonViewModel()
      {
         Name = new(person.Firstname, person.Lastname),
         BirthDate = 
      }
   }

   public Task SaveModelAsync(PersonViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }

   private static EventViewModel CreateEventViewModel(LifeStoryMember lifeStoryMember, LifeStory lifeStory)
   {
      return new EventViewModel(
         new EventDate(lifeStoryMember.Date ?? lifeStory.Date ?? ""),
         Location: lifeStory.Location);
   }

   private bool TryGetBirth(Data.Models.Person person, out LifeStory lifeStory, out string? date)
   {
      foreach (var lifeStoryMember in person.LifeStories)
      {
         var lifeStory = _crudableRepository.GetLifeStory(lifeStoryMember.LifeStoryId);
         if(lifeStory.Type == LifeStoryType.Födelse)
         {

         }

         switch (lifeStory.Type)
         {
            case LifeStoryType.Födelse:
               viewModel.Birth = CreateEventViewModel(lifeStoryMember, lifeStory);
               break;

            case LifeStoryType.Död:
               viewModel.Death = CreateEventViewModel(lifeStoryMember, lifeStory);
               break;
         }
      }
   }
}