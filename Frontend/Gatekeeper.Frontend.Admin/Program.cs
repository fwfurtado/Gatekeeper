using System.Net.Http.Headers;
using Gatekeeper.Frontend.Admin;
using Gatekeeper.Frontend.Admin.Auth;
using Gatekeeper.Frontend.Admin.Services;
using Gatekeeper.Frontend.Admin.Validations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

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

builder.Services.AddSingleton<AuthHandler>();
builder.Services.AddScoped<CpfValidator>();
builder.Services.AddScoped<ResidentFormValidator>();
builder.Services.AddScoped<UnitFormValidator>();
builder.Services.AddScoped<PackageFormValidator>();
builder.Services.AddScoped<ResidentService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();