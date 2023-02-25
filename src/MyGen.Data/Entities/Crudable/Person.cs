namespace MyGen.Data.Entities;

public class Person : ICrudable
{
   public Guid Id { get; init; }

   public required string Firstname { get; set; }
   public required string Lastname { get; set; }
   public required string Sex { get; set; }

   public string? Profession { get; set; }
   public string? Notes { get; set; }

   public IList<FamilyMember>? Families { get; set; }
   public IList<LifeStoryMember>? LifeStories { get; set; }
   public IList<Guid>? MediaIds { get; set; }

   public override int GetHashCode()
   {
      return new
      {
         Id,
         Firstname,
         Lastname,
         Sex,
         Profession,
         Notes,
         Families = EntityRepository.GetHashCodeFromCollection(Families),
         LifeStories = EntityRepository.GetHashCodeFromCollection(LifeStories),
         MediaIds = EntityRepository.GetHashCodeFromCollection(MediaIds)
      }.GetHashCode();
   }

   int ICrudable.Version => 1;
}