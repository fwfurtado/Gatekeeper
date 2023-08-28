using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class ResidentFaker : Faker<Resident>
{
    public ResidentFaker()
    {
        CustomInstantiator(f => new Resident(f.Person.FullName, new CpfFaker().Generate()));
    }
}