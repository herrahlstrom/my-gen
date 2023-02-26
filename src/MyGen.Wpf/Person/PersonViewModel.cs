using MyGen.Shared;
using MyGen.Shared.Definitions;
using MyGen.Wpf.Shared;
using System;
using System.Collections.Generic;

namespace MyGen.Wpf.Person;

internal class PersonViewModel : IViewModel<PersonUserControl>, IMainTabViewModel, IPerson
{
   private readonly IViewModelRepository<PersonViewModel> _repository;

   public PersonViewModel(IViewModelRepository<PersonViewModel> repository)
   {
      _repository = repository;
   }

   public EventViewModel Birth { get; set; }
   DateModel IPerson.BirthDate { get => Birth.Date; }
   public EventViewModel Death { get; set; }
   DateModel IPerson.DeathDate { get => Death?.Date; }
   public IList<PersonFamily> Families { get; set; } = Array.Empty<PersonFamily>();
   public IPerson? Father { get; set; }
   public string FullName => Name.FirstName + " " + Name.LastName;
   public object? Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
   public Guid Id { get; private set; }
   object IMainTabViewModel.Id => Id;
   public IPerson? Mother { get; set; }
   public PersonName Name { get; set; } = PersonName.Empty;
   public string Notes { get; set; } = "";
   public string Profession { get; set; } = "";
   public Sex Sex { get; set; } = Sex.Unknown;
   string IMainTabViewModel.Title => Name.GivenName + " " + Name.LastName;

   public void Load(object? argument = null)
   {
      if (argument is not Guid id)
      {
         throw new ArgumentException(paramName: nameof(argument), message: "Passed argument must be of type Guid");
      }

      Id = id;
      _repository.LoadModel(this);
   }
}

internal record EventViewModel(DateModel Date, string? Location = null);

internal record PersonFamily(IPerson Partner, IList<IPerson> Children);