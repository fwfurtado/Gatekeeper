using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Entities;

public class ApartmentTest
{
    [Test]
    public void IdentifierCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Apartment(""));
        Assert.Throws<ArgumentException>(() => new Apartment(null));
    }

    [Test]
    public void ShouldCreateApartmentWithAllData()
    {
        var apartment = new Apartment("1");

        Assert.AreEqual("1", apartment.Identifier);
        CollectionAssert.IsEmpty(apartment.Tenants);
        
    }


    [Test]
    public void ShouldAddTenantsToApartment()
    {
        Assert.Fail("Not implemented Yet!");
    }
}