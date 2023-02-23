using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MyGen.Data.Models;

public class Media : ICrudable
{
   public string? FileCrc { get; set; }
   public Guid Id { get; set; }

   public List<MediaMeta>? Meta { get; set; }

   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
   public bool Missing { get; set; }

   public string? Notes { get; set; }
   public string Path { get; set; } = null!;

   public long Size { get; set; }
   public string? Title { get; set; }
   public MediaType Type { get; set; }
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new
      {
         Id,
         Path,
         Type,
         Meta = CrudableRepository.GetHashCodeFromCollection(Meta),
         Missing,
         Notes,
         Size,
         Title,
         FileCrc
      }.GetHashCode();
   }
}

[DebuggerDisplay("{Key} {Value}")]
public class MediaMeta
{
   public string Key { get; set; }
   public string Value { get; set; }
}
