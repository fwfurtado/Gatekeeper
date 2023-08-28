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
    public void DescriptionCannotBeBlank()
    {
        var tenant = new ResidentFaker().Generate();
        Assert.Throws<ArgumentException>(() => new Mail(tenant, ""));
    }

    [Test]
    public void ShouldCreateMailWithAllData()
    {
        var description = _faker.Commerce.ProductDescription();
        var tenant = new ResidentFaker().Generate();
        var mail = new Mail(tenant, description);

        mail.Resident.Should().Be(tenant);
        mail.Description.Should().Be(description);
        mail.ArrivedAt.Should().BeBefore(DateTime.UtcNow);
        
    }
}