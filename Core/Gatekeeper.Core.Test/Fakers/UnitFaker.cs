using Bogus;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class UnitFaker : Faker<Unit>
{
    public UnitFaker(bool generateId = true)
    {

        if (generateId)
        {
            CustomInstantiator(f => new Unit(f.Random.Long(min: 1), f.Address.BuildingNumber()));
        }
        else
        {
            CustomInstantiator(f => new Unit( f.Address.BuildingNumber()));
        }
    }
}