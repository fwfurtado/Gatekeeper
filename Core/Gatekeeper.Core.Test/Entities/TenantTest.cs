using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Entities;

public class TenantTest
{
    private readonly Faker _faker = new();

    [Test]
    public void NameCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Tenant("", _faker.Person.Cpf()));
        Assert.Throws<ArgumentException>(() => new Tenant(null, _faker.Person.Cpf()));
    }

    [Test]
    public void DocumentCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Tenant(_faker.Person.FirstName, ""));
        Assert.Throws<ArgumentException>(() => new Tenant(_faker.Person.FirstName, null));
    }

    [Test]
    public void ShouldCreateTenantWithAllData()
    {
        var name = _faker.Person.FirstName;
        var document = _faker.Person.Cpf();
        var tenant = new Tenant(name, document);
    
        tenant.Name.Should().Be(name);
        tenant.Document.Should().Be(document);
    }
}