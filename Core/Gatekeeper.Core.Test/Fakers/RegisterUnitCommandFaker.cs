using Bogus;
using Gatekeeper.Core.Commands;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class RegisterUnitCommandFaker : Faker<RegisterUnitCommand>
{
    public RegisterUnitCommandFaker()
    {
        CustomInstantiator(f => new RegisterUnitCommand(f.Address.BuildingNumber()));
    }
}