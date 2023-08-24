using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public class TenantFaker : Faker<Tenant>
{
    public TenantFaker()
    {
        CustomInstantiator(f => new Tenant(f.Person.FullName, f.Person.Cpf()));
    }
}
