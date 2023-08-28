using Bogus;
using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;

namespace Gatekeeper.Core.Test.Entities;

public class UnitTest
{
    private readonly Faker _faker = new();

    [Test]
    public void IdentifierCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Unit(""));
        Assert.Throws<ArgumentException>(() => new Unit(null));
    }

    [Test]
    public void ShouldCreateApartmentWithAllData()
    {
        var identifier = _faker.Address.BuildingNumber();
        var apartment = new Unit(identifier);

       apartment.Identifier.Should().Be(identifier);
       apartment.Residents.Should().BeEmpty();
        
    }


    [Test]
    public void ShouldAddTenantsToApartment()
    {
        var identifier = _faker.Address.BuildingNumber();
        var apartment = new Unit(identifier);
        var tenant = new TenantFaker().Generate();

        apartment.Residents.Add(tenant);
        
        apartment.Residents.Should().NotBeEmpty()
            .And.HaveCount(1);
    }
}