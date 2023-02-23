namespace MyGen.Data.Models;

public class LifeStory : ICrudable
{
   public Guid Id { get; set; }
   public string? Name { get; set; }
   public string? Date { get; set; }
   public string? EndDate { get; set; }
   public string? Location { get; set; }
   public string Notes { get; set; } = "";
   public List<Guid>? SourceIds { get; set; }
   public LifeStoryType Type { get; set; }
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new
      {
         Id,
         Name,
         Date,
         EndDate,
         Location,
         Notes,
         SourceIds = CrudableRepository.GetHashCodeFromCollection(SourceIds),
         Type
      }.GetHashCode();
   }
}
