using MyGen.Shared;

namespace MyGen.Wpf.Shared;

public interface IPerson
{
   EventDate BirthDate { get; }
   EventDate DeathDate { get; }
   object? Icon { get; }
   PersonName Name { get; }
}