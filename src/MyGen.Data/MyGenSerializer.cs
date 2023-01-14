using MyGen.Data.Models;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MyGen.Data;

internal class MyGenSerializer
{
   private readonly JsonSerializerOptions _jsonSerializerOptions;

   public MyGenSerializer()
   {
      _jsonSerializerOptions = new JsonSerializerOptions
      {
         WriteIndented = true,
         DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
      };
   }


   public ICrudable Read(Stream stream, out IDictionary<string, string> meta)
   {
      var reader = new StreamReader(stream, Encoding.UTF8);

      meta = new Dictionary<string, string>();
      while (true)
      {
         var line = reader.ReadLine();
         if (string.IsNullOrEmpty(line)) { break; }
         var p = line.IndexOf(':');
         meta.Add(line[..p], line[(p + 1)..]);
      }

      string type = meta["type"];
      int version = int.Parse(meta["version"]);

      return type switch
      {
         nameof(Person) => JsonSerializer.Deserialize<Person>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Family) => JsonSerializer.Deserialize<Family>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Source) => JsonSerializer.Deserialize<Source>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(LifeStory) => JsonSerializer.Deserialize<LifeStory>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         nameof(Media) => JsonSerializer.Deserialize<Media>(reader.ReadToEnd(), _jsonSerializerOptions)!,
         _ => throw new InvalidDataException($"Can't deserialize type {type}")
      };
   }

   public void Write<T>(Stream stream, T entity) where T : ICrudable
   {
      var writer = new StreamWriter(stream, Encoding.UTF8);
      writer.WriteLine("type:{0}", entity.GetType().Name);
      writer.WriteLine("version:{0}", entity.Version);
      writer.WriteLine("updated:{0:s}", DateTime.UtcNow);
      writer.WriteLine();

      var json = JsonSerializer.Serialize(entity, entity.GetType(), _jsonSerializerOptions);
      writer.WriteLine(json);

      writer.Flush();
   }
}