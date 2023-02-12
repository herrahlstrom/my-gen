using Microsoft.Extensions.DependencyInjection;
using MyGen.Wpf.Shared;
using System;
using System.Windows.Controls;

namespace MyGen.Wpf;

internal class ViewModelFactory
{
   private readonly IServiceProvider _serviceProvider;

   public ViewModelFactory(IServiceProvider serviceProvider)
   {
      _serviceProvider = serviceProvider;
   }

   public IViewModel<T> CreateViewModelFor<T>() where T : ContentControl
   {
      return _serviceProvider.GetRequiredService<IViewModel<T>>();
   }
}