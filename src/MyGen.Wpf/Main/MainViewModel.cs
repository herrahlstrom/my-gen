using MyGen.Wpf.Person;
using MyGen.Wpf.Shared;
using MyGen.Wpf.Tools;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyGen.Wpf.Main;

internal class MainViewModel : ViewModelBase, IViewModel<MainWindow>
{
   private readonly ViewModelFactory _vmFactory;

   private IMainTabViewModel? _selectedTab = null;

   public MainViewModel(ViewModelFactory vmFactory, AppState appState)
   {
      _vmFactory = vmFactory;

      Search = _vmFactory.CreateViewModelFor<SearchUserControl>();

      appState.OpenPerson.SetRequestCallback(id =>
      {
         if (_vmFactory.CreateViewModelFor<PersonUserControl>() is IMainTabViewModel viewModel)
         {
            viewModel.Load(id);
            Open(viewModel);
         }
      });
   }

   public ObservableCollection<IMainTabViewModel> OpenTabs { get; } = new();

   public IViewModel<SearchUserControl> Search { get; }

   public IMainTabViewModel? SelectedTab
   {
      get => _selectedTab;
      set
      {
         _selectedTab = value;
         OnPropertyChanged();
      }
   }

   public string Title => "My Gen";

   public void Load(object? _)
   {
      Search.Load();
   }

   public void Open<TModel>(TModel model) where TModel : IMainTabViewModel
   {
      if (OpenTabs.OfType<TModel>().FirstOrDefault(x => x.Id.Equals(model.Id)) is { } existing)
      {
         OpenTabs.Remove(existing);
      }
      OpenTabs.Insert(0, model);
      SelectedTab = model;
   }
}