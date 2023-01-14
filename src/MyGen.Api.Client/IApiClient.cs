using MyGen.Api.Shared.Models;
using Refit;

namespace MyGen.Api.Client;
public interface IApiClient
{
   [Get("/person/{personId}/lifestories")]
   Task<IEnumerable<LifeStoryDto>> GetLifeStoriesOnPerson(Guid personId);

   [Get("/lifestory/{id}")]
   Task<LifeStoryDto> GetLifeStoryAsync(Guid id);

   [Get("/media/{id}")]
   Task<MediaDto> GetMediaAsync(Guid id);

   [Get("/person/{personId}/media")]
   Task<IEnumerable<MediaDto>> GetMediaOnPerson(Guid personId);

   [Get("/person/{id}")]
   Task<PersonDto> GetPersonAsync(Guid id);

   [Get("/person/find?filter={filter}")]
   Task<IEnumerable<PersonDto>> GetPersonsAsync(string filter);
}