using System.Net.Http.Json;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Services;

public class ResidentService
{
    private const string BaseEndpoint = "residents";
    private readonly HttpClient _client;

    public ResidentService(HttpClient client)
    {
        _client = client;
    }
    
    
    public async Task<IEnumerable<ResidentResponse>> GetAllAsync()
    {
        var result =  await _client.GetFromJsonAsync<IEnumerable<ResidentResponse>>(BaseEndpoint);
        
        return result ?? Enumerable.Empty<ResidentResponse>();
    }
    
    public async Task<bool> SaveAsync(ResidentForm form)
    {

        var request = new ResidentRequest()
        {
            Name = form.Name,
            Document = form.Document
        };
        
        var response = await _client.PostAsJsonAsync(BaseEndpoint, request);
        
        return response.IsSuccessStatusCode;
    }
}