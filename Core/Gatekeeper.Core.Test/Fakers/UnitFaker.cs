using Bogus;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class UnitFaker : Faker<Unit>
{
    public UnitFaker()
    {
        CustomInstantiator(f => new Unit(f.Address.BuildingNumber()));
    }
}