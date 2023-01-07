using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace MyGen.Data.Models;

public class Person : ICrudable
{
   public Guid Id { get; init; }

   public required string Firstname { get; set; }
   public required string Lastname { get; set; }
   public required string Sex { get; set; }

   public string Profession { get; set; } = "";
   public string Notes { get; set; } = "";

   public required ICollection<FamilyMember> Families { get; init; }
   public required ICollection<LifeStoryMember> LifeStories { get; init; }
   public required IList<Guid> MediaIds { get; init; }

   public override int GetHashCode()
   {
      return new { Id, Firstname, Lastname, Sex, Profession, Notes, Families, LifeStories, MediaIds }.GetHashCode();
   }

   int ICrudable.Version => 1;
}