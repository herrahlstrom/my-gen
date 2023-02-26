using MyGen.Shared.Definitions;

namespace MyGen.Data.Entities;

public class LifeStoryMember
{
   public Guid PersonId { get; set; }

   public string? Date { get; set; }

   public string? EndDate { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(PersonId, Date, EndDate);
   }
}
public class LifeStoryFamily
{
   public Guid FamilyId { get; set; }
   public override int GetHashCode()
   {
      return HashCode.Combine(FamilyId);
   }
}


public class PersonLifeStory
{
   public Guid LifeStoryId { get; set; }

   public LifeStoryType Type { get; set; }

   public string? Date { get; set; }

   public string? EndDate { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(LifeStoryId, Date, EndDate, Type);
   }
}


public class FamilyLifeStory
{
   public Guid LifeStoryId { get; set; }

   public LifeStoryType Type { get; set; }

   public override int GetHashCode()
   {
      return HashCode.Combine(LifeStoryId, Type);
   }
}
