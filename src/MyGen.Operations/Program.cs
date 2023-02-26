using Microsoft.Extensions.Logging.Abstractions;
using MyGen.Data;
using MyGen.Data.Entities;
using MyGen.Operations;

var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
var repo = new EntityRepository(fs, new NullLogger<EntityRepository>());
repo.Load();

var persons = repo.GetEntities<Person>();

//foreach (var person in persons)
//{
//   if (person.LifeStories is null) { continue; }

//   foreach (var lifeStoryMember in person.LifeStories)
//   {
//      if(lifeStoryMember.Type == MyGen.Shared.Definitions.LifeStoryType.None)
//      {
//         var lifeStory = repo.GetEntity<LifeStory>(lifeStoryMember.LifeStoryId);
//         lifeStoryMember.Type = lifeStory.Type;
//      }
//   }
//}

//foreach (var person in persons)
//{
//   if (person.Families is null) { continue; }

//   foreach (var familyMember in person.Families)
//   {
//      if (familyMember.PersonId == Guid.Empty)
//      {
//         familyMember.PersonId = person.Id;
//      }

//      var family = repo.GetEntity<Family>(familyMember.FamilyId);
//      family.Members ??= new List<FamilyMember>();
//      if (!family.Members.Any(x => x.PersonId == familyMember.PersonId))
//      {
//         family.Members.Add(familyMember);
//      }
//   }
//}

repo.Save();