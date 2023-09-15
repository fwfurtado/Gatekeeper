using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;
using static FluentAssertions.FluentActions;
namespace Gatekeeper.Core.Test.Entities;

public class UnitTest
{
    private readonly UnitFaker _faker = new();

    [Test]
    public void IdentifierCannotBeBlank()
    {
        Invoking(() => new Unit("")).Should().Throw<ArgumentException>();
    }
}