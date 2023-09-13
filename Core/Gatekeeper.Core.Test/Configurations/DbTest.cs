using Gatekeeper.Core.Configurations;
using Gatekeeper.Shared.Database;
using Gatekeeper.Shared.Test;
using Microsoft.Extensions.Configuration;

namespace Gatekeeper.Core.Test.Configurations;

[SetUpFixture]
public abstract class DbTest : DatabaseTest
{
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