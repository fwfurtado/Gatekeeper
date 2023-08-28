using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Entities;

public class ResidentTest
{
    private readonly Faker _faker = new();

    [Test]
    public void NameCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Resident("", _faker.Person.Cpf()));
        
    }

    [Test]
    public void DocumentCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Resident(_faker.Person.FirstName, ""));
    }

    [Test]
    public void ShouldCreateTenantWithAllData()
    {
        var name = _faker.Person.FirstName;
        var document = _faker.Person.Cpf();
        var tenant = new Resident(name, document);
    
        tenant.Name.Should().Be(name);
        tenant.Document.Should().Be(document);
    }
}