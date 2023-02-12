using MyGen.Wpf.Shared;
using System.Threading.Tasks;

namespace MyGen.Wpf.Main;

internal class MainViewModelRepository : IViewModelRepository<MainViewModel>
{
   public Task LoadModelAsync(MainViewModel viewModel)
   {
      return Task.CompletedTask;
   }

   public Task SaveModelAsync(MainViewModel viewModel)
   {
      throw new System.NotImplementedException();
   }
}