using MyGen.Shared;

namespace MyGen.Wpf.Shared
{
   public class SlimPersonViewModel : IPerson
   {
      public required PersonName Name { get; set; }
      public required EventDate BirthDate { get; set; }
      public required EventDate DeathDate { get; set; }

      // ToDo: Fix icon from picture of person or generic gener icon
      public object? Icon { get; set; }
   }
}
