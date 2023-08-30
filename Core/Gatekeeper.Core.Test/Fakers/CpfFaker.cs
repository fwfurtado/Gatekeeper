using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Test.Fakers;

public sealed class CpfFaker : Faker<Cpf>
{
    public CpfFaker()
    {
        CustomInstantiator(f => f.Person.Cpf(includeFormatSymbols: false));
    }
}