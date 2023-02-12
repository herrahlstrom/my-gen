using MyGen.Wpf.Infrastructure;
using MyGen.Wpf.Person;
using MyGen.Wpf.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyGen.Wpf.Main;

internal class MainViewModel : ViewModelBase, IViewModel<MainWindow>
{
   private readonly IFactory<MainWindow> _viewFactory;
   private readonly ViewModelFactory _vmFactory;

   private IViewModel? _selectedTab = null;

   public MainViewModel(IFactory<MainWindow> viewFactory, ViewModelFactory vmFactory)
   {
      _viewFactory = viewFactory;
      _vmFactory = vmFactory;
   }

   object IViewModel.Id => Guid.Empty;

   public ObservableCollection<IViewModel> OpenTabs { get; } = new();

   public IViewModel? SelectedTab
   {
      get => _selectedTab;
      set
      {
         _selectedTab = value;
         OnPropertyChanged();
      }
   }

   public string Title => "My Gen";

   public MainWindow CreateView()
   {
      var view = _viewFactory.Create();
      view.DataContext = this;
      return view;
   }

   public async Task LoadAsync(object? _)
   {
      IViewModel<PersonUserControl> pVm = _vmFactory.CreateViewModelFor<PersonUserControl>();
      await pVm.LoadAsync(Guid.Empty);

      Open(pVm);
   }

   public void Open<TModel>(TModel model) where TModel : IViewModel
   {
      if (OpenTabs.OfType<TModel>().FirstOrDefault(x => x.Id.Equals(model.Id)) is { } existing)
      {
         OpenTabs.Remove(existing);
      }
      OpenTabs.Insert(0, model);
      SelectedTab = model;
   }
}