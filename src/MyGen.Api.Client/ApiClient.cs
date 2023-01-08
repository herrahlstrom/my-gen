﻿using MyGen.Api.Shared.Models;
using System.Net.Http.Json;
using System.Web;

namespace MyGen.Api.Client;
public class ApiClient : IApiClient, IDisposable
{
   readonly HttpClient _http;

   public ApiClient(IHttpClientFactory httpFactory)
   {
      _http = httpFactory.CreateClient(nameof(ApiClient));
   }

   public void Dispose()
   {
      _http.Dispose();
   }

   public async Task<PersonDto?> GetPersonAsync(Guid id)
   {
      return await _http.GetFromJsonAsync<PersonDto>($"person/{id}");
   }
   public async Task<IEnumerable<PersonDto>> GetPersonsAsync(string filter)
   {
      System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString("");
      query.Add("filter", filter);

      return await _http.GetFromJsonAsync<PersonDto[]>($"person/find?{query}") ?? Array.Empty<PersonDto>();
   }
}