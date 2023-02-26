using MyGen.Shared;

namespace MyGen.Wpf.Shared;

public interface IPerson
{
   DateModel BirthDate { get; }
   DateModel DeathDate { get; }
   object? Icon { get; }
   PersonName Name { get; }
}