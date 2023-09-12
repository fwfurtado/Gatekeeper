using FluentAssertions;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Test.Fakers;
using static FluentAssertions.FluentActions;

namespace Gatekeeper.Core.Test.Entities;

public class ResidentTest
{

    [Test]
    public void NameCannotBeBlank()
    {
        var faker = new CpfFaker();
        
        Invoking(() => new Resident("", faker.Generate())).Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void NameCannotBeBlankInSecondaryConstructor()
    {
        var faker = new CpfFaker();
        
        Invoking(() => new Resident(1, faker.Generate(), "")).Should().Throw<ArgumentException>();
    }
}