using MyGen.Shared;
using MyGen.Wpf.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyGen.Wpf.Person;

internal class PersonViewModel : IViewModel<PersonUserControl>, IMainTabViewModel, IPerson
{
   private readonly IViewModelRepository<PersonViewModel> _repository;

   public PersonViewModel(IViewModelRepository<PersonViewModel> repository)
   {
      _repository = repository;
   }

   public EventViewModel Birth { get; set; }
   EventDate IPerson.BirthDate { get => Birth.Date; }
   public EventViewModel? Death { get; set; }
   EventDate IPerson.DeathDate { get => Death?.Date; }
   public IList<PersonFamily> Families { get; set; } = Array.Empty<PersonFamily>();
   public IPerson? Father { get; set; }
   public string FullName => Name.FirstName + " " + Name.LastName;
   public Guid Id { get; private set; }
   object IMainTabViewModel.Id => Id;
   public IPerson? Mother { get; set; }
   public PersonName Name { get; set; } = PersonName.Empty;
   public string Notes { get; set; } = "";
   public string Profession { get; set; } = "";
   public Sex Sex { get; set; } = Sex.Unknown;
   string IMainTabViewModel.Title => Name.GivenName + " " + Name.LastName;
   public object? Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

internal record EventViewModel(EventDate Date, string? Location = null);

internal record PersonFamily(IPerson Partner, IList<IPerson> Children);