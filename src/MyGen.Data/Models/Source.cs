using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace MyGen.Data.Models;

[DebuggerDisplay("{Id} | {ReferenceId} | {Name} | {Url}")]
public class Source : ICrudable
{
   public Guid Id { get; set; }
   public string Name { get; set; } = "";
   public string? Repository { get; set; }
   public string? Volume { get; set; }
   public string? Page { get; set; }
   public string? Url { get; set; }
   public string? ReferenceId { get; set; }
   public string? ImagePath { get; set; }
   public string Notes { get; set; } = "";

   public SourceType Type { get; set; }

   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new { Id, Name, Repository, Volume, Page, Url, ReferenceId, ImagePath, Notes, Type }.GetHashCode();
   }
}

public enum SourceType
{
   None = 0,

   [Display(Name = "Annan källa")]
   Other = 1,

   [Display(Name = "Riksarkivet")]
   Riksarkivet = 2,

   [Display(Name = "Arkiv Digital")]
   ArkivDigital = 3
}