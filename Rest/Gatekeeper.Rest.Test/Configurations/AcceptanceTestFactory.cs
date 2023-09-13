using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Gatekeeper.Rest.Test.Configurations;

public class AcceptanceTestFactory: WebApplicationFactory<Program> 
{
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("AcceptanceTest");
    }
    
}