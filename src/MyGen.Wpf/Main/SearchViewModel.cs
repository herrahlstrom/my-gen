using MyGen.Shared;
using MyGen.Wpf.Shared;
using MyGen.Wpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Data;
using System.Windows.Input;

namespace MyGen.Wpf.Main;

internal class SearchViewModel : IViewModel<SearchUserControl>
{
   private readonly IViewModelRepository<SearchViewModel> _repository;
   private Timer _autoSearchTimer;
   private string _filter = "";
   private string[] _filterComponents = Array.Empty<string>();
   private List<SearchResult> _result;

   public SearchViewModel(IViewModelRepository<SearchViewModel> repository, AppState appState)
   {
      _repository = repository;
      _result = new();

      ResultView = new ListCollectionView(_result)
      {
         Filter = FilterItem
      };
      ResultView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SearchResult.GroupName)));

      _autoSearchTimer = new();
      _autoSearchTimer.Elapsed += (_, _) =>
      {
         _autoSearchTimer.Enabled = false;
         App.Current.Dispatcher.Invoke(ResultView.Refresh);
      };

      OpenPersonCommand = new RelayCommand<Guid>(
         execute: appState.OpenPerson.Request);
   }

   public string Filter
   {
      get { return _filter; }
      set
      {
         _autoSearchTimer.Enabled = false;

         _filter = value ?? "";
         _filterComponents = _filter.Split((string?)null, StringSplitOptions.RemoveEmptyEntries);

         _autoSearchTimer.Interval = _filter.Length >= 3 ? 50 : 750;
         _autoSearchTimer.Start();
      }
   }

   public object Id => throw new NotImplementedException();
   public ICommand OpenPersonCommand { get; }
   public ListCollectionView ResultView { get; }
   public string Title => throw new NotImplementedException();

   public void Load(object? argument = null)
   {
      _repository.LoadModel(this);
   }

   public void SetResult(IEnumerable<SearchResult> items)
   {
      _result.Clear();
      _result.AddRange(items);
      ResultView.Refresh();
   }

   private bool FilterItem(object obj)
   {
      if (_filterComponents.Length == 0)
      {
         return false;
      }

      return obj switch
      {
         PersonSearchResult person => _filterComponents.All(w =>
            person.Name.FirstName.Contains(w, StringComparison.OrdinalIgnoreCase) ||
            person.Name.LastName.Contains(w, StringComparison.OrdinalIgnoreCase)),

         _ => false
      };
   }
}

abstract record SearchResult()
{
   public abstract string GroupName { get; }
}

record PersonSearchResult(Guid Id, PersonName Name) : SearchResult
{
   public override string GroupName => "Personer";
   public string DisplayName => $"{Name.FirstName} {Name.LastName}";
}