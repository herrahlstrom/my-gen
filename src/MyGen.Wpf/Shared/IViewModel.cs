using System.Windows.Controls;

namespace MyGen.Wpf.Shared;

internal interface IViewModel
{
   public void Load(object? argument = null);
}

internal interface IViewModel<T> : IViewModel where T : ContentControl
{
}