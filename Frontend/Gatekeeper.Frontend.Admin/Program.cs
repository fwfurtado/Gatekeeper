using Gatekeeper.Frontend.Admin;
using Gatekeeper.Frontend.Admin.Services;
using Gatekeeper.Frontend.Admin.Validations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5032") });
builder.Services.AddScoped<CpfValidator>();
builder.Services.AddScoped<ResidentFormValidator>();
builder.Services.AddScoped<ResidentService>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();