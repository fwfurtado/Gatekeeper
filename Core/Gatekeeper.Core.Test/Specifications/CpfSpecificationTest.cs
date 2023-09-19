using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Specifications;

namespace Gatekeeper.Core.Test.Specifications;

public class CpfSpecificationTest
{
    private readonly CpfSpecification _cpfSpecification = new();

    [TestCase("")]
    [TestCase("12345678901")]
    [TestCase("1234")]
    [TestCase("1A345678901")]
    public void ShouldReturnFalseWhenCpfIsInvalid(string cpf)
    {
        var result = _cpfSpecification.IsValid(cpf);

        Assert.That(result, Is.False);
    }

    [TestCaseSource(nameof(ValidCpf), new object[] { 30 })]
    public void ShouldReturnTrueWhenCpfIsValid(string cpf)
    {
        var result = _cpfSpecification.IsValid(cpf);

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