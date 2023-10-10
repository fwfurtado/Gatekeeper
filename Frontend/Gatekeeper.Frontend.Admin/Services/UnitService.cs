using System.Net.Http.Json;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Services;

public class UnitService
{
    private const string BaseEndpoint = "units";
    private readonly HttpClient _client;

    public UnitService(HttpClient client)
    {
        _client = client;
    }


    public async Task<IEnumerable<UnitResponse>> GetAllAsync()
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<UnitResponse>>(BaseEndpoint);

        return result ?? Enumerable.Empty<UnitResponse>();
    }

    public async Task<bool> SaveAsync(UnitForm form)
    {

        var request = new UnitRequest()
        {
            Identifier = form.Identifier
        };

        var response = await _client.PostAsJsonAsync(BaseEndpoint, request);

        return response.IsSuccessStatusCode;
    }
}