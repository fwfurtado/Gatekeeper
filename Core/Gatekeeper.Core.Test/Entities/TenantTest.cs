using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Entities;

public class TenantTest
{
    [Test]
    public void NameCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Tenant("", "123456"));
        Assert.Throws<ArgumentException>(() => new Tenant(null, "123456"));
    }

    [Test]
    public void DocumentCannotBeBlank()
    {
        Assert.Throws<ArgumentException>(() => new Tenant("Morador", ""));
        Assert.Throws<ArgumentException>(() => new Tenant("Morador", null));
    }

    [Test]
    public void ShouldCreateTenantWithAllData()
    {
        var tenant = new Tenant("Morador", "1234");
    
        Assert.AreEqual("Morador", tenant.Name);
        Assert.AreEqual("1234", tenant.Document);
    }
}