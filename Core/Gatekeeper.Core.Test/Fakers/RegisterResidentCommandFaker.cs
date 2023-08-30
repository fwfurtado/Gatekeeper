using Bogus;
using Gatekeeper.Core.Commands;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class RegisterResidentCommandFaker : Faker<RegisterResidentCommand>
{
    public RegisterResidentCommandFaker()
    {
        CustomInstantiator(f => new RegisterResidentCommand(f.Person.FullName, new CpfFaker().Generate()));
    }
}