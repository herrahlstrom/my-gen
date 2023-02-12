using MyGen.Wpf.Shared;
using System.Threading.Tasks;

namespace MyGen.Wpf.Person;

internal class PersonViewModelRepository : IViewModelRepository<PersonViewModel>
{
   public Task LoadModelAsync(PersonViewModel viewModel)
   {
      viewModel.Name = new("Ove Martin* Sebastian", "Ahlström");

      return Task.CompletedTask;
   }

   public Task SaveModelAsync(PersonViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }
}