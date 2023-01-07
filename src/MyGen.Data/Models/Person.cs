namespace MyGen.Data.Models;

public class Person : ICrudable
{
   public Guid Id { get; init; }

   public required string Firstname { get; set; }
   public required string Lastname { get; set; }
   public required string Sex { get; set; }

   public string Profession { get; set; } = "";
   public string Notes { get; set; } = "";

   public ICollection<FamilyMember> Families { get; init; } = Array.Empty<FamilyMember>();
   public ICollection<LifeStoryMember> LifeStories { get; init; } = Array.Empty<LifeStoryMember>();
   public IList<Guid> MediaIds { get; init; } = Array.Empty<Guid>();

   public override int GetHashCode()
   {
      return new { Id, Firstname, Lastname, Sex, Profession, Notes, Families, LifeStories, MediaIds }.GetHashCode();
   }

   int ICrudable.Version => 1;
}