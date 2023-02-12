using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyGen.Wpf.Shared;

internal interface IViewModel
{
   object Id { get; }
   public string Title { get; }

   public Task LoadAsync(object? argument = null);
}

internal interface IViewModel<T> : IViewModel where T : ContentControl
{
   T CreateView();
}