using MyGen.Wpf.Infrastructure;
using MyGen.Wpf.Shared;
using System;
using System.Threading.Tasks;

namespace MyGen.Wpf.Person;

internal class PersonViewModel : IViewModel<PersonUserControl>
{
   private readonly IViewModelRepository<PersonViewModel> _repository;
   private IFactory<PersonUserControl> _viewFactory;

   public PersonViewModel(IFactory<PersonUserControl> viewFactory, IViewModelRepository<PersonViewModel> repository)
   {
      _viewFactory = viewFactory;
      _repository = repository;
   }

   public string FullName { get; set; } = "";
   public Guid Id { get; private set; }
   object IViewModel.Id => Id;
   public string ShortName { get; set; } = "";

   string IViewModel.Title => ShortName;
   public PersonUserControl CreateView()
   {
      var view = _viewFactory.Create();
      view.DataContext = this;
      return view;
   }

   public async Task LoadAsync(object? argument = null)
   {
      if (argument is not Guid id)
      {
         throw new ArgumentException(paramName: nameof(argument), message: "Passed argument must be of type Guid");
      }

      Id = id;
      await _repository.LoadModelAsync(this);
   }
}