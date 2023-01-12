using MyGen.Api.Shared.Models;
using Refit;

namespace MyGen.Api.Client;
public interface IApiClient
{
   [Get("/person/{id}")]
   Task<PersonDto> GetPersonAsync(Guid id);

   [Get("/person/find?filter={filter}")]
   Task<IEnumerable<PersonDto>> GetPersonsAsync(string filter);

   [Get("/lifestory/{id}")]
   Task<LifeStoryDto> GetLifeStoryAsync(Guid id);

   [Get("/person/{personId}/lifestories")]
   Task<IEnumerable<LifeStoryDto>> GetLifeStoriesOnPerson(Guid personId);
}