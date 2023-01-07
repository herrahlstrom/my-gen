namespace MyGen.Data.Models;

public class LifeStoryMember
{
   /// <summary>
   ///     Only if override from LifeStory (parent)
   /// </summary>
   public string? Date { get; set; }

   /// <summary>
   ///     Only if override from LifeStory (parent)
   /// </summary>
   public string? EndDate { get; set; }

   public Guid LifeStoryId { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(LifeStoryId, Date, EndDate);
   }
}
