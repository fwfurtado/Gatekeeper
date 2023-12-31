﻿using System.Net.Http.Json;
using System.Web;
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


    public async Task<PagedResponse<UnitResponse>> GetAllAsync(PageRequest pageRequest)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["Page"] = pageRequest.Page.ToString();
        query["Size"] = pageRequest.Size.ToString();

        var queryString = query.ToString();
        
        var result = await _client.GetFromJsonAsync<PagedResponse<UnitResponse>>($"{BaseEndpoint}?{queryString}");

        return result ?? new PagedResponse<UnitResponse>();
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

    public async Task<IEnumerable<UnitResponse>> GetAllListAsync()
    {
        var result = await _client.GetFromJsonAsync<IEnumerable<UnitResponse>>(BaseEndpoint);

        return result ?? Enumerable.Empty<UnitResponse>();
    }

    public async Task<IEnumerable<UnitResponse>> FilterByIdentifierAsync(string text)
    {
        var url = $"{BaseEndpoint}/filter?identifier={text}";

        if (string.IsNullOrEmpty(text))
        {
            url = $"{BaseEndpoint}/filter";
        }

        var result = await _client.GetFromJsonAsync<IEnumerable<UnitResponse>>(url);

        return result ?? Enumerable.Empty<UnitResponse>();
    }
}