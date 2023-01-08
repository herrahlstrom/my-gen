using MyGen.Data;
using MyGen.Data.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IFileSystem>((_) => new FileSystem(@"C:\Users\marti\source\repos\my-gen-data"));
builder.Services.AddSingleton<CrudableRepository>();

var app = builder.Build();

// Load repository
app.Services.GetRequiredService<CrudableRepository>().Load();

app.MapGet("person/{id:guid}", (CrudableRepository repository, Guid id) => repository.GetEntity<Person>(id));
app.MapGet("person/find", (CrudableRepository repository, string filter) => repository.GetEntities<Person>().Where(x => Match(x, filter)));

app.Run();

bool Match(Person p, string Filter)
{
   return Filter.ToLower().Split().All(word =>
      p.Firstname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
      p.Lastname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
      p.Profession.Contains(word, StringComparison.OrdinalIgnoreCase) ||
      p.Notes.Contains(word, StringComparison.OrdinalIgnoreCase)
  );
}
