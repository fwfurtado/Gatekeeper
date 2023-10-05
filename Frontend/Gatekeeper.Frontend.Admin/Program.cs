using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Gatekeeper.Frontend.Admin;
using MudBlazor.Services;
using Gatekeeper.Core.Validations;
using Gatekeeper.Core.Specifications;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5032") });
builder.Services.AddScoped<ICpfSpecification, CpfSpecification>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();