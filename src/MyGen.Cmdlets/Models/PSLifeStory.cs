namespace MyGen.Cmdlets.Models;

public class PSLifeStory
{
   public required Guid Id { get; init; }
   public required string Name { get; set; }
   public required string Date { get; set; }
   public required string EndDate { get; set; }
   public required string Location { get; set; }
   public required string Notes { get; set; }
   public required KeyValuePair<int, string> Type { get; set; }
}