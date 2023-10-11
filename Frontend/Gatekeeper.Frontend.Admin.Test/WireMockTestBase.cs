using WireMock.Net.Testcontainers;

namespace Gatekeeper.Frontend.Admin.Test;

public abstract class WireMockTestBase
{
    protected readonly WireMockContainer WireMockContainer = new WireMockContainerBuilder()
        .WithMappings(TestHelper.MappingsPath)
        .WithAutoRemove(true)
        .WithCleanUp(true)
        .Build();


    [OneTimeSetUp]
    public async Task Setup()
    {
        await WireMockContainer.StartAsync();
    }


    [OneTimeTearDown]
    public async Task TearDown()
    {
        await WireMockContainer.StopAsync();
        await WireMockContainer.DisposeAsync();
    }
}