using System.Net.Http.Headers;
using Gatekeeper.Frontend.Admin;
using Gatekeeper.Frontend.Admin.Auth;
using Gatekeeper.Frontend.Admin.Services;
using Gatekeeper.Frontend.Admin.Validations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Unleash;
using Unleash.ClientFactory;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = "http://localhost:8080/realms/gatekeeper/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "http://localhost:8080/realms/gatekeeper";
    options.ProviderOptions.ClientId = "backoffice";
    options.ProviderOptions.ResponseType = "code";
});

builder.Services.AddHttpClient("Gatekeeper.Rest.Api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5032");
    
}).AddHttpMessageHandler<AuthHandler>();
    

builder.Services.AddScoped(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();

    return factory.CreateClient("Gatekeeper.Rest.Api");
});


var settings = new UnleashSettings
{
    AppName = "dotnet-test",
    UnleashApi = new Uri("http://localhost:4242/api/"),
    CustomHttpHeaders = new ()
    {
        {"Authorization","default:development.unleash-insecure-api-token" }
    }
};


builder.Services.AddSingleton<UnleashSettings>(_ => settings);
builder.Services.AddScoped<IUnleash>(c => new UnleashClientFactory().CreateClient(c.GetRequiredService<UnleashSettings>()));
builder.Services.AddSingleton<AuthHandler>();
builder.Services.AddScoped<CpfValidator>();
builder.Services.AddScoped<ResidentFormValidator>();
builder.Services.AddScoped<UnitFormValidator>();
builder.Services.AddScoped<ResidentService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();