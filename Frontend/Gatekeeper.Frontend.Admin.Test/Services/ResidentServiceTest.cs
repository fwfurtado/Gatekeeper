using Gatekeeper.Frontend.Admin.Dtos;
using Gatekeeper.Frontend.Admin.Services;

namespace Gatekeeper.Frontend.Admin.Test.Services;

public class ResidentServiceTest : WireMockTestBase
{

    private ResidentService _service = null!;


    [SetUp]
    public void SetupEach()
    {
        var client = WireMockContainer.CreateClient();
        _service = new ResidentService(client);
    }


    [Test]
    public async Task ShouldGetAllResidents()
    {
        var residentsEnumerable = await _service.GetAllAsync();
        var residents = residentsEnumerable.ToList();

        Assert.That(residents, Is.Not.Null);
        Assert.That(residents, Is.Not.Empty);
    }

    [Test]
    public async Task ShouldSaveANewResident()
    {
        var request = new ResidentForm
        {
            Name = "John Lion",
            Document = "85540054067"
        };

        var success = await _service.SaveAsync(request);

        Assert.That(success, Is.True);
    }
}