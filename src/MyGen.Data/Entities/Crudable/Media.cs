﻿using MyGen.Shared.Definitions;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MyGen.Data.Entities;

public class Media : ICrudable
{
   public Guid Id { get; set; }
   public MediaType Type { get; set; }
   public List<MediaMeta>? Meta { get; set; }

   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
   public bool Missing { get; set; }
   public string Path { get; set; } = null!;
   public long Size { get; set; }
   public string? Title { get; set; }
   public string? FileCrc { get; set; }
   public string? Notes { get; set; }
   int ICrudable.Version => 1;

   public override int GetHashCode()
   {
      return new
      {
         Id,
         Path,
         Type,
         Meta = EntityRepository.GetHashCodeFromCollection(Meta),
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
   public required string Key { get; set; }
   public required string Value { get; set; }
}
