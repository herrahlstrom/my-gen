using Microsoft.Extensions.Logging.Abstractions;
using MyGen.Data;
using MyGen.Data.Entities;
using MyGen.Operations;

var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
var repo = new EntityRepository(fs, new NullLogger<EntityRepository>());
repo.Load();

//repo.Save();