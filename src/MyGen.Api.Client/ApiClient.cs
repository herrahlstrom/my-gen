using MyGen.Api.Shared.Models;
using System.Net.Http.Json;
using System.Web;

namespace MyGen.Api.Client;

public class ApiClient : IApiClient, IDisposable
{
   private readonly HttpClient _http;

   public ApiClient(IHttpClientFactory httpFactory)
   {
      _http = httpFactory.CreateClient(nameof(ApiClient));
   }

   public void Dispose()
   {
      _http.Dispose();
   }

   public async Task<LifeStoryDto> GetLifeStoryAsync(Guid id)
   {
      return await Get<LifeStoryDto>($"lifestory/{id}");
   }

   public async Task<PersonDto> GetPersonAsync(Guid id)
   {
      return await Get<PersonDto>($"person/{id}");
   }

   public async Task<IEnumerable<LifeStoryDto>> GetLifeStoriesOnPerson(Guid personId)
   {
      return await Get<LifeStoryDto[]>($"person/{personId}/lifestories");
   }

   public async Task<IEnumerable<PersonDto>> GetPersonsAsync(string filter)
   {
      System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString("");
      query.Add("filter", filter);

      return await Get<PersonDto[]>($"person/find?{query}");
   }

   private async Task<T> Get<T>(string path)
   {
      return await _http.GetFromJsonAsync<T>(path) ?? throw new Exception($"{path} returned null");
   }
}