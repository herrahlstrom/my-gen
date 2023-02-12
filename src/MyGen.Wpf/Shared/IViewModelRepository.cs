using System.Threading.Tasks;

namespace MyGen.Wpf.Shared;

internal interface IViewModelRepository<TViewModel>
{
   Task LoadModelAsync(TViewModel viewModel);

   Task SaveModelAsync(TViewModel viewModel);
}