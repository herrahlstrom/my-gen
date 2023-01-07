using MyGen.Data;

var fs = new FileSystem("C:\\Users\\marti\\source\\repos\\my-gen-data");
var repo = new CrudableRepository(fs);
repo.Load();
