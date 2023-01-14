namespace MyGen.Data.Models;

public class Family : ICrudable
{
   public Guid Id { get; init; }
   public List<LifeStoryMember>? LifeStories { get; set; }
   public string Notes { get; set; } = "";
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new
      {
         Id,
         LifeStories = CrudableRepository.GetHashCodeFromCollection(LifeStories),
         Notes
      }.GetHashCode();
   }
}