using Gatekeeper.Core.Configurations;

namespace Gatekeeper.Core.Test.Configurations;

public class MappingTest
{
    [Test]
    public void ConfigurationShouldBeValid()
    {
        var configuration = AutoMapperConfiguration.Configure();
        configuration.AssertConfigurationIsValid();
    }
}