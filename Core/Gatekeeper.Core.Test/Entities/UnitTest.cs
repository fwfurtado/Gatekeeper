using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;
using static FluentAssertions.FluentActions;
namespace Gatekeeper.Core.Test.Entities;

public class UnitTest
{
    private readonly UnitFaker _faker = new();

    [Test]
    public void IdentifierCannotBeBlank()
    {
        Invoking(() => new Unit("")).Should().Throw<ArgumentException>();
    }

    [Test]
    public void ShouldCreateApartmentWithAllData()
    {
        var unit = _faker.Generate();
        unit.Residents.Should().BeEmpty();
    }


    [Test]
    public void ShouldAddTenantsToApartment()
    {
        var unit = _faker.Generate();
        var resident = new ResidentFaker().Generate();

        unit.AssociateResident(resident);

        unit.Residents
            .Should()
            .HaveCount(1)
            .And.ContainSingle(r => r.Name == resident.Name && r.Document == resident.Document);
    }
}