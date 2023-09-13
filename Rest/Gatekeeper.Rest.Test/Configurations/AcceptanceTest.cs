using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Shared.Database;
using Gatekeeper.Shared.Test;
using Microsoft.Extensions.Configuration;

namespace Gatekeeper.Rest.Test.Configurations;

[SetUpFixture]
public abstract class AcceptanceTest: DatabaseTest
{
    public const string BaseUrl = "http://localhost:5032";
    
    private AcceptanceTestFactory _factory = null!;
    protected AcceptanceTestFactory Factory => _factory;
    
    protected string JwtToken => JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
        new JwtSecurityToken(
            JwtTokenProvider.Issuer,
            JwtTokenProvider.Issuer,
            new List<Claim> { new(ClaimTypes.Role, "Operator"), new("department", "Security") },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: JwtTokenProvider.SigningCredentials
        )
    ); 
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", ConnectionString);
        _factory = new AcceptanceTestFactory();
    }
    
    protected override IDbConnectionFactory GetConnectionFactory()
    {
        var dataSource = new Dictionary<string, string>()
        {
            { "DATABASE_CONNECTION_STRING", ConnectionString }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(dataSource!)
            .Build();

        return new DbConnectionFactory(configuration);
    }
}