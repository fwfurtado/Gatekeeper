﻿using FluentAssertions;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Dtos;
using Gatekeeper.Rest.Test.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gatekeeper.Rest.Test.Controllers;

[TestFixture]
public class ResidentControllerTest : AcceptanceTest
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

    [Test]
    public async Task ShouldCreateAResident()
    {
        var request = new
        {
            Name = "resident-name",
            Document = new Cpf("41615990054")
        };

        var createdResponse = await _httpClient.PostAsJsonAsync("residents", request);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBodyResponse = await createdResponse.Content.ReadAsStringAsync();
        createdBodyResponse.Should().BeEmpty();

        var path = createdResponse.Headers.Location!.PathAndQuery;
        var showResponse = await _httpClient.GetAsync(path);
        showResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var showBodyResponse = await showResponse.Content.ReadAsStringAsync();
        var resident = JsonSerializer.Deserialize<ResidentResponse>(showBodyResponse);
        resident.Should().NotBeNull();
        resident!.Name.Should().Be(request.Name);
        resident!.Document.Should().Be(request.Document);
    }

    [Test]
    public async Task ShouldReturnConflictWhenSavingSameDocument()
    {
        var request = new
        {
            Name = "resident-name",
            Document = new Cpf("41615990054")
        };

        await _httpClient.PostAsJsonAsync("residents", request);
        var createdResponse = await _httpClient.PostAsJsonAsync("residents", request);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Test]
    public async Task ShouldReturnBadRequestWhenWrongDocument()
    {
        var request = new
        {
            Name = "resident-name",
            Document = "12345678901"
        };

        var createdResponse = await _httpClient.PostAsJsonAsync("residents", request);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenResidentDoesNotExist()
    {
        var showResponse = await _httpClient.GetAsync("residents/1");
        showResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}