using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Gatekeeper.Core.Test.Fakers;
using Gatekeeper.Core.ValueObjects;

namespace Gatekeeper.Core.Test.ValueObject;

public class CpfTest
{
    private readonly Faker _faker = new();

    [Test]
    public void ShouldBePossibleToConvertStringToCpf()
    {
        var cpfAsString = _faker.Person.Cpf(includeFormatSymbols: false);
        
        Cpf cpf = cpfAsString;

        cpf.Number.Should().BeEquivalentTo(cpfAsString);
    }

    [Test]
    public void ShouldBePossibleToConvertACpfToString()
    {
        var cpf = new CpfFaker().Generate();
        
        string cpfAsString = cpf;
        
        cpfAsString.Should().BeEquivalentTo(cpf.Number);
    }
}