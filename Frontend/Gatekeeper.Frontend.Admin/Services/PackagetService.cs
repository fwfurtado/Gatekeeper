using System.Net.Http.Json;
using System.Web;
using Gatekeeper.Frontend.Admin.Dtos;

namespace Gatekeeper.Frontend.Admin.Services;

public class PackageService
{
    private const string BaseEndpoint = "packages";
    private readonly HttpClient _client;

    public PackageService(HttpClient client)
    {
        _client = client;
    }
    
    
    public async Task<PagedResponse<PackageResponse>> GetAllAsync(PageRequest pageRequest)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["Page"] = pageRequest.Page.ToString();
        query["Size"] = pageRequest.Size.ToString();

        var queryString = query.ToString();

        var result =  await _client.GetFromJsonAsync<PagedResponse<PackageResponse>>($"{BaseEndpoint}?{queryString}");
        
        return result ?? new PagedResponse<PackageResponse>();
    }
    
    public async Task<bool> SaveAsync(PackageForm form)
    {

        var request = new PackageRequest()
        {
            Description = form.Description,
            UnitId = form.UnitId
        };
        
        var response = await _client.PostAsJsonAsync(BaseEndpoint, request);
        
        return response.IsSuccessStatusCode;
    }
}