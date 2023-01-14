using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyGen.Data;
using MyGen.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
var repo = new CrudableRepository(fs);
repo.Load();

using (ServiceProvider sp = new ServiceCollection()
   .AddDbContext<MyGen.Data.Legacy.AppDbContext>((optionsBuilder) => optionsBuilder.UseSqlite("Data Source=G:\\Min enhet\\Släktforskning\\Bilder\\.db\\db.sqlite;"))
   .BuildServiceProvider())
{
   var persons = repo.GetEntities<Person>().ToList();
   var sources = repo.GetEntities<Source>().ToList();
   var db = sp.GetRequiredService<MyGen.Data.Legacy.AppDbContext>();

   foreach (var image in db.Images)
   {
      var imageMeta = db.ImageMeta.Where(x => x.ImageId == image.Id).ToList();

      var media = new Media()
      {
         Id = Guid.NewGuid(),
         Type = image.TypeId.ToString().ToUpper() switch
         {
            "CF1461D0-F25D-4847-A180-B91395EEDE67" => MediaType.Potrait,
            "E6F3710D-5BEE-4470-8891-6349AF720DD1" => MediaType.Source,
            _ => MediaType.Unknown
         },
         Path = image.Path,
         Title = image.Title is { Length: > 0 } title ? title : null,
         Notes = image.Notes is { Length: > 0 } notes ? notes : null,
         Size = image.Size,
         FileCrc = image.FileCrc is { Length: > 0 } crc ? crc : null
      };

      if (imageMeta.Count > 0)
      {
         if (media.Type == MediaType.Source)
         {
            if (TryGetSource(imageMeta, out var source))
            {
               source.MediaIds ??= new List<Guid>();
               source.MediaIds.Add(media.Id);

               if (source.Name == "") { source.Name = null; }
               if (source.Notes == "") { source.Notes = null; }
               if (source.Page == "") { source.Page = null; }
               if (source.ReferenceId == "") { source.ReferenceId = null; }
               if (source.Repository == "") { source.Repository = null; }
               if (source.Url == "") { source.Url = null; }
               if (source.Volume == "") { source.Volume = null; }
            }
         }
         media.Meta ??= new List<MediaMeta>();
         media.Meta.AddRange(imageMeta.Where(x=> x.Value != "").Select(meta => new MediaMeta { Key = meta.Key, Value = meta.Value }));
      }

      repo.AddEntity(media);

      var personNamesInImage = (from pi in db.PersonImages
                                join p in db.Persons on pi.PersonId equals p.Id
                                where pi.ImageId == image.Id
                                select p.Name).ToList();
      foreach (var personName in personNamesInImage)
      {
         var matchPersons = GetPersons(personName);
         if (matchPersons.Count > 0)
         {
            if (matchPersons.Count > 1)
            {
               media.Notes = new StringBuilder(media.Notes ?? "")
                  .AppendLine()
                  .AppendLine("Vilken person är rätt?")
                  .AppendFormat("\"{0}\"", personName.Trim())
                  .ToString().Trim();
            }
            foreach (var p in matchPersons)
            {
               p.MediaIds ??= new List<Guid>();
               p.MediaIds.Add(media.Id);

               if (p.Profession == "") { p.Profession = null; }
               if (p.Notes == "") { p.Notes = null; }
            }
         }
         else
         {
            throw new Exception($"Hittar inte person: {personName}");
         }
      }
   }

   IList<Person> GetPersons(string name)
   {
      List<Person> exactMatch = persons!.Where(x => $"{x.Firstname} {x.Lastname}" == name).ToList();
      if (exactMatch.Count > 0)
      {
         return exactMatch;
      }

      IEnumerable<Person> q = persons!;
      foreach (var element in name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim('(', ')', '*')))
      {
         q = q.Where(x =>
            x.Firstname.Contains(element, StringComparison.OrdinalIgnoreCase) ||
            x.Lastname.Contains(element, StringComparison.OrdinalIgnoreCase));
      }
      var looseMatch = q.ToList();
      if (looseMatch.Count > 0)
      {
         return looseMatch;
      }

      return Array.Empty<Person>();
   }

   bool TryGetSource(ICollection<MyGen.Data.Legacy.ImageMeta> imageMeta, [MaybeNullWhen(false)] out Source? source)
   {
      List<Source> sourceCandidates = new List<Source>(sources);

      if (imageMeta.Where(x => x.Key == "Reference" && x.Value != "").ToList() is { Count: 1 } referenceMeta)
      {
         sourceCandidates.RemoveAll(x => x.ReferenceId != referenceMeta[0].Value);
         if (sourceCandidates.Count == 1)
         {
            source = sourceCandidates[0];
            return true;
         }
      }

      if (imageMeta.Where(x => x.Key == "Page" && x.Value != "").ToList() is { Count: 1 } pageMeta)
      {
         sourceCandidates.RemoveAll(x => x.Page != pageMeta[0].Value);
         if (sourceCandidates.Count == 1)
         {
            source = sourceCandidates[0];
            return true;
         }
      }

      if (imageMeta.Where(x => x.Key == "Repository" && x.Value != "").ToList() is { Count: 1 } repositoryMeta)
      {
         sourceCandidates.RemoveAll(x => x.Repository != repositoryMeta[0].Value);
         if (sourceCandidates.Count == 1)
         {
            source = sourceCandidates[0];
            return true;
         }
      }

      if (imageMeta.Where(x => x.Key == "Volume" && x.Value != "").ToList() is { Count: 1 } volumeMeta)
      {
         sourceCandidates.RemoveAll(x => x.Volume != volumeMeta[0].Value);
         if (sourceCandidates.Count == 1)
         {
            source = sourceCandidates[0];
            return true;
         }
      }

      source = default;
      return false;
   }
}

repo.Save();

//var me = repo.GetEntities<Person>()
//   .Where(x => x.Firstname.Contains("martin", StringComparison.OrdinalIgnoreCase) &&
//               x.Firstname.Contains("sebastian", StringComparison.OrdinalIgnoreCase) &&
//               x.Lastname.Contains("ahlström", StringComparison.OrdinalIgnoreCase))
//   .Single();
//me.Profession = "Mjukvaruutvecklare";
//me.Notes ??= "";
//me.Notes += "Detta är ett test";


//var konrad = repo.GetEntities<Person>()
//   .Where(x => x.Firstname.Contains("konrad", StringComparison.OrdinalIgnoreCase) &&
//               x.Lastname.Contains("ahlström", StringComparison.OrdinalIgnoreCase))
//   .Single();
//repo.RemoveEntity(konrad);

//repo.Save();