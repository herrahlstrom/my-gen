namespace MyGen.Data.Models;

public class Family : ICrudable
{
   public Guid Id { get; init; }
   public required ICollection<LifeStoryMember> LifeStories { get; init; }
   public string Notes { get; set; } = "";
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return HashCode.Combine(Id, LifeStories, Notes );
   }
}