using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class TenantFaker : Faker<Resident>
{
    public TenantFaker()
    {
        CustomInstantiator(f => new Resident(f.Person.FullName, f.Person.Cpf()));
    }
}
