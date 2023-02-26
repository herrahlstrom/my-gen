using System.Threading.Tasks;

namespace MyGen.Wpf.Shared;

internal interface IViewModelRepository<TViewModel>
{
   void LoadModel(TViewModel viewModel);

   void SaveModel(TViewModel viewModel);
}