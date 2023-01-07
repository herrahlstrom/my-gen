using System.ComponentModel.DataAnnotations;

namespace MyGen.Data.Models;

public class LifeStory : ICrudable
{
   public Guid Id { get; set; }
   public string? Name { get; set; }
   public string? Date { get; set; }
   public string? EndDate { get; set; }
   public string? Location { get; set; }
   public string Notes { get; set; } = "";
   public required IList<Guid> SourceIds { get; init; }
   public LifeStoryType Type { get; set; }
   int ICrudable.Version => 1;
    
   public override int GetHashCode()
   {
      return HashCode.Combine(Id, Name, Date, EndDate, Location, Notes, SourceIds, Type);
   }
}

public enum LifeStoryType
{
   None = 0,

   [Display(Name = "Födelse")] Födelse = 1,
   [Display(Name = "Död")] Död = 2,
   [Display(Name = "Döpt")] Döpt = 3,
   [Display(Name = "Boende")] Boende = 4,
   [Display(Name = "Boupptäckning")] Boupptäckning = 5,
   [Display(Name = "Begravd")] Begravd = 6,
   [Display(Name = "Övrigt")] Övrigt = 7,
   [Display(Name = "Yrke")] Yrke = 8,
   [Display(Name = "Förlovad")] Förlovad = 9,
   [Display(Name = "Gift")] Gift = 10,
   [Display(Name = "Skiljd")] Skiljd = 11,
   [Display(Name = "Separerad")] Separerad = 12,
   [Display(Name = "Förälder")] Förälder = 13,
   [Display(Name = "Flytt")] Flytt = 14,
   [Display(Name = "Dömd")] Dömd = 15,
}