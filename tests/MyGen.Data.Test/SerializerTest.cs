using MyGen.Data.Entities;
using MyGen.Shared.Definitions;

namespace MyGen.Data.Test;

[TestClass]
public class SerializerTest
{
   [TestMethod]
   public void Serialize_Family()
   {
      var model = new Family()
      {
         Id = Guid.NewGuid(),
         LifeStories = new List<LifeStoryMember>
         {
            new LifeStoryMember(){LifeStoryId = Guid.NewGuid()},
            new LifeStoryMember(){LifeStoryId = Guid.NewGuid()}
         },
         Notes = "Sample notes"
      };

      TestSerializer(model);
   }

   [TestMethod]
   public void Serialize_LifeStory()
   {
      var model = new LifeStory()
      {
         Id = Guid.NewGuid(),
         Date = "2021-01-01",
         EndDate = "2022-01-01",
         Location = "Mölndal",
         Name = "Dummy name",
         Type = LifeStoryType.Boende,
         SourceIds = new List<Guid>
         {
            Guid.NewGuid(),
         }
      };

      TestSerializer(model);
   }
   
   [TestMethod]
   public void Serialize_Media()
   {
      var model = new Media()
      {
         Id = Guid.NewGuid(),
         Path = "path/to/media",
         FileCrc = "abc123",
         Missing = true,
         Notes = "notes",
         Size = 1234,
         Title = "title",
         Type = MediaType.Potrait,
         Meta = new List<MediaMeta>()
         {
            new MediaMeta()
            {
               Key = "Key",
               Value = "Value"
            }
         }
      };

      TestSerializer(model);
   }

   [TestMethod]
   public void Serialize_Person()
   {
      var model = new Person()
      {
         Id = Guid.NewGuid(),
         Firstname = "Martin",
         Lastname = "Ahlström",
         Sex = "M",
         Profession = "Software developer",
         Notes = "Test notes",
         Families = new List<FamilyMember>
         {
            new FamilyMember() { FamilyId = Guid.NewGuid(), MemberType = FamilyMemberType.Husband },
         },
         LifeStories = new List<LifeStoryMember>
         {
            new LifeStoryMember(){LifeStoryId = Guid.NewGuid(), Date = "1984-09-05"},
            new LifeStoryMember(){LifeStoryId = Guid.NewGuid(), Date = "1984-09-05", EndDate = "2007-02-11"}
         },
         MediaIds = new List<Guid>
         {
            Guid.NewGuid(),
            Guid.NewGuid(),
         }
      };

      TestSerializer(model);
   }

   [TestMethod]
   public void Serialize_Source()
   {
      var model = new Source()
      {
         Id = Guid.NewGuid(),
         Name = "A II a 17 (1850-1894) | p 137 | a123_4",
         Repository = "Fässberg (O)",
         Volume = "A II a 17 (1850-1894)",
         Page = "137",
         ReferenceId = "a123_4",
         Notes = "Sample text",
         Type = SourceType.Other,
         Url = "http://www.source.net",
         MediaIds = new List<Guid>
         {
            Guid.NewGuid(),
            Guid.NewGuid(),
         }
      };

      TestSerializer(model);
   }

   private static void TestSerializer<T>(T model) where T : ICrudable
   {
      ICrudable recreatedModel;
      IDictionary<string, string> meta;

      using (var stream = new MemoryStream())
      {
         var serializer = new MyGenSerializer();
         serializer.Write(stream, model);

         stream.Position = 0;
         recreatedModel = serializer.Read(stream, out meta);
      }

      recreatedModel.Should().BeEquivalentTo(model);

      meta.TryGetValue("version", out string? versionString).Should().BeTrue();
      int.TryParse(versionString, out int version).Should().BeTrue();
      version.Should().BePositive();

      meta.TryGetValue("updated", out string? updatedString).Should().BeTrue();
      DateTime.TryParse(updatedString, out DateTime dateTime).Should().BeTrue();
      dateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
   }
}
