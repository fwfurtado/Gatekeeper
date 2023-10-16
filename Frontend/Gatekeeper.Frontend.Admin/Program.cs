using Gatekeeper.Frontend.Admin;
using Gatekeeper.Frontend.Admin.Services;
using Gatekeeper.Frontend.Admin.Validations;
using Microsoft.AspNetCore.Components.Web;
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

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5032") });
builder.Services.AddScoped<CpfValidator>();
builder.Services.AddScoped<ResidentFormValidator>();
builder.Services.AddScoped<UnitFormValidator>();
builder.Services.AddScoped<ResidentService>();
builder.Services.AddScoped<UnitService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();