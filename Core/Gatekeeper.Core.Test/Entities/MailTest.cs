using Bogus;
using Bogus.DataSets;
using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;

namespace Gatekeeper.Core.Test.Entities;


public class MailTest
{
    private readonly Faker _faker = new();

    [Test]
    public void TenantCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Mail(null, _faker.Commerce.ProductDescription()));
    }

    [Test]
    public void DescriptionCannotBeBlank()
    {
        var tenant = new TenantFaker().Generate();
        Assert.Throws<ArgumentException>(() => new Mail(tenant, ""));
        Assert.Throws<ArgumentException>(() => new Mail(tenant, null));
    }

    [Test]
    public void ShouldCreateMailtWithAllData()
    {
        var description = _faker.Commerce.ProductDescription();
        var tenant = new TenantFaker().Generate();
        var mail = new Mail(tenant, description);

        mail.Tenant.Should().Be(tenant);
        mail.Description.Should().Be(description);
        mail.ArrivedAt.Should().BeBefore(DateTime.UtcNow);
        
    }
}