using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class MailFaker : Faker<Mail>
{
    public MailFaker(Tenant tenant)
    {
        CustomInstantiator(f => new Mail(tenant, f.Commerce.ProductDescription()));
    }
}
