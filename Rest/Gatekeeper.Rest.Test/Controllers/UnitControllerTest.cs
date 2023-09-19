using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using FluentAssertions;
using Gatekeeper.Rest.Dtos;
using Gatekeeper.Rest.Test.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
                BaseAddress = new Uri(BaseUrl),
            }
        );
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            JwtToken
        );
    }
    
    [TearDown]
    public void AfterEach()
    {
        _httpClient.Dispose();
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
    
    [Test]
    public async Task ShouldReturnNotFoundWhenUnitDoesNotExist()
    {
        var showResponse = await _httpClient.GetAsync("units/1");

        showResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}