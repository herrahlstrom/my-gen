using MyGen.Data;
using MyGen.Shared;
using MyGen.Shared.Extensions;
using MyGen.Wpf.Shared;
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

      var person = _crudableRepository.GetEntity<Data.Models.Person>(viewModel.Id);

      viewModel.Name = new(person.Firstname, person.Lastname);
      viewModel.Sex = Mapper.ToSex(person.Sex);
      viewModel.Profession = person.Profession ?? "";
      viewModel.Notes = person.Notes ?? "";
   }

   public Task SaveModelAsync(PersonViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }
}