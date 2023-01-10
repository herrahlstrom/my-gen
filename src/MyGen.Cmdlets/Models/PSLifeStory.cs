namespace MyGen.Cmdlets.Models;

public class PSLifeStory
{
   public Guid Id { get; set; }
   public string Name { get; set; }
   public string Date { get; set; }
   public string EndDate { get; set; }
   public string Location { get; set; }
   public string Notes { get; set; }
   public KeyValuePair<int, string> Type { get; set; }
}