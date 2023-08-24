using Bogus;
using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;

namespace Gatekeeper.Core.Test.Entities;

public class ApartmentTest
{
    private readonly Faker _faker = new();

    [Test]
    public void IdentifierCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Apartment(""));
        Assert.Throws<ArgumentException>(() => new Apartment(null));
    }

    [Test]
    public void ShouldCreateApartmentWithAllData()
    {
        var identifier = _faker.Address.BuildingNumber();
        var apartment = new Apartment(identifier);

       apartment.Identifier.Should().Be(identifier);
       apartment.Tenants.Should().BeEmpty();
        
    }


    [Test]
    public void ShouldAddTenantsToApartment()
    {
        var identifier = _faker.Address.BuildingNumber();
        var apartment = new Apartment(identifier);
        var tenant = new TenantFaker().Generate();

        apartment.Tenants.Add(tenant);
        
        apartment.Tenants.Should().NotBeEmpty()
            .And.HaveCount(1);
    }
}