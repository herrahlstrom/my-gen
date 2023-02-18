using MyGen.Shared;
using MyGen.Wpf.Shared;
using System;
using System.Threading.Tasks;

namespace MyGen.Wpf.Person;

internal class PersonViewModel : IViewModel<PersonUserControl>, IMainTabViewModel
{
   private readonly IViewModelRepository<PersonViewModel> _repository;

   public PersonViewModel(IViewModelRepository<PersonViewModel> repository)
   {
      _repository = repository;
   }

   public string FullName => Name.FirstName + " " + Name.LastName;
   public Guid Id { get; private set; }
   object IMainTabViewModel.Id => Id;
   public PersonName Name { get; set; } = PersonName.Empty;
   string IMainTabViewModel.Title => Name.GivenName + " " + Name.LastName;

   public string Notes { get; set; } = "";
   public string Profession { get; set; } = "";
   public Sex Sex { get; set; } = Sex.Unknown;
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