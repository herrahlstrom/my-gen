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

app.MapGet("lifestory/{id:guid}", (CrudableRepository repository, Guid id) =>
   repository.TryGetEntity(id, out LifeStory ls)
      ? Results.Json(Mapper.ToDto(ls, null))
      : Results.NotFound());

app.MapGet("person/find", (CrudableRepository repository, string filter) =>
   repository.GetPersons().Where(x => Match(x, filter)).Select(Mapper.ToDto));

app.MapGet("person/{id:guid}/lifestories", (CrudableRepository repository, Guid id) =>
{
   if (!repository.TryGetEntity(id, out Person p))
   {
      return Results.NotFound();
   }

   List<LifeStoryDto> result = new();
   if (p.LifeStories is not null)
   {
      foreach (var ls in p.LifeStories)
      {
         var lifeStory = repository.GetLifeStory(ls.LifeStoryId);
         result.Add(Mapper.ToDto(lifeStory, ls));
      }
   }
   return Results.Json(result);
});

app.MapGet("person/{id:guid}/media", (CrudableRepository repository, Guid id) =>
{
   if (!repository.TryGetEntity(id, out Person p))
   {
      return Results.NotFound();
   }

   List<MediaDto> result = new();
   if (p.MediaIds is not null)
   {
      foreach (Guid mediaId in p.MediaIds)
      {
         var media = repository.GetMedia(mediaId);
         result.Add(Mapper.ToDto(media));
      }
   }
   return Results.Json(result);
});


app.Run();

bool Match(Person p, string Filter)
{
   return Filter.ToLower().Split().All(word =>
      p.Firstname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
      p.Lastname.Contains(word, StringComparison.OrdinalIgnoreCase) ||
      p.Profession?.Contains(word, StringComparison.OrdinalIgnoreCase) == true ||
      p.Notes?.Contains(word, StringComparison.OrdinalIgnoreCase) == true
  );
}
