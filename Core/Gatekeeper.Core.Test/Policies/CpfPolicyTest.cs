using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Policies;

namespace Gatekeeper.Core.Test.Policies;

public class CpfPolicyTest
{
    private readonly CpfPolicy _cpfPolicy = new();

    [TestCase("12345678901")]
    public void ShouldReturnFalseWhenCpfIsInvalid(string cpf)
    {
        var result = _cpfPolicy.IsValid(cpf);

        Assert.That(result, Is.False);
    }

    [TestCaseSource(nameof(ValidCpf), new object[] { 30 })]
    public void ShouldReturnTrueWhenCpfIsValid(string cpf)
    {
        var result = _cpfPolicy.IsValid(cpf);

        Assert.That(result, Is.True);
    }


    private static IEnumerable<string> ValidCpf(int count)
    {
        var faker = new Faker();

        for (var i = 0; i < count; i++)
        {
            yield return faker.Person.Cpf(false);
        }
    }
}