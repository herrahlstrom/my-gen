namespace MyGen.Cmdlets.Models;

public class PSPerson
{
   public required Guid Id { get; set; }
   public required string Firstname { get; set; }
   public required string Lastname { get; set; }
   public required string Sex { get; set; }
   public required string Profession { get; set; }
   public required string Notes { get; set; }
}