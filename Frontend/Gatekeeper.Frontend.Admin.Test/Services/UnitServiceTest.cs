using Gatekeeper.Frontend.Admin.Dtos;
using Gatekeeper.Frontend.Admin.Services;

namespace Gatekeeper.Frontend.Admin.Test.Services;

public class UnitServiceTest: WireMockTestBase
{
    
    private UnitService _service = null!;


    [SetUp]
    public void SetupEach()
    {
        var client = WireMockContainer.CreateClient();
        _service = new UnitService(client);
    }


    [Test]
    public async Task ShouldGetAllUnits()
    {
        var unitsEnumerable = await _service.GetAllAsync();
        var units = unitsEnumerable.ToList();

        Assert.That(units, Is.Not.Null);
        Assert.That(units, Is.Not.Empty);
    }

    [Test]
    public async Task ShouldSaveANewUnit()
    {
        var request = new UnitForm
        {
            Identifier = "Apt 101"
        };

        var success = await _service.SaveAsync(request);

        Assert.That(success, Is.True);
    }
}