namespace MyGen.Cmdlets.Models;

public class PSMedia
{
   public required Guid Id { get; init; }
   public required string Path { get; set; }
   public required long Size { get; set; }
   public required string Title { get; set; }
}