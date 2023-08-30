using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class MailFaker : Faker<Mail>
{
    public MailFaker(Resident resident)
    {
        CustomInstantiator(f => new Mail(resident, f.Commerce.ProductDescription()));
    }
}
