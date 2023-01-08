using MyGen.Api.Shared.Models;

namespace MyGen.Api.Client;
public interface IApiClient
{
    Task<PersonDto?> GetPersonAsync(Guid id);
    Task<IEnumerable<PersonDto>> GetPersonsAsync(string filter);
}