using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Rest.Controllers;
using Gatekeeper.Rest.Dtos;
using Gatekeeper.Rest.Test.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;


namespace Gatekeeper.Rest.Test.Controllers;

[TestFixture]
public class UnitControllerTest : AcceptanceTest
{
    private HttpClient _httpClient = null!;

    [SetUp]
    public void Setup()
    {
        _httpClient = Factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(BaseUrl)
            }
        );
    }


    [Test]
    public async Task ShouldCreateAnUnit()
    {
        var request = new
        {
            Identifier = "unit-identifier"
        };

        var createdResponse = await _httpClient.PostAsJsonAsync("units", request);

        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBodyResponse = await createdResponse.Content.ReadAsStringAsync();

        createdBodyResponse.Should().BeEmpty();


        var path = createdResponse.Headers.Location!.PathAndQuery;
        
        
        var showResponse = await _httpClient.GetAsync(path);

        showResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var showBodyResponse = await showResponse.Content.ReadAsStringAsync();
        
        var unit = JsonSerializer.Deserialize<UnitResponse>(showBodyResponse);
        
        unit.Should().NotBeNull();
        unit!.Identifier.Should().Be(request.Identifier);
        
    }
}