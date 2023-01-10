using MyGen.Api;
using MyGen.Api.Shared.Models;
using MyGen.Data;
using MyGen.Data.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IFileSystem>((_) => new FileSystem(@"C:\Users\marti\source\repos\my-gen-data"));
builder.Services.AddSingleton<CrudableRepository>();

var app = builder.Build();

// Load repository
app.Services.GetRequiredService<CrudableRepository>().Load();

app.MapGet("person/{id:guid}", (CrudableRepository repository, Guid id) =>
   repository.TryGetEntity(id, out Person p)
      ? Results.Json(Mapper.ToDto(p))
      : Results.NotFound());


app.MapGet("person/find", (CrudableRepository repository, string filter) =>
   repository.GetEntities<Person>().Where(x => Match(x, filter)).Select(Mapper.ToDto));
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
