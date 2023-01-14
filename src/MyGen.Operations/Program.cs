using MyGen.Data;
using MyGen.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
var repo = new CrudableRepository(fs);
repo.Load();

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