using FluentAssertions;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Test.Configurations;
using Gatekeeper.Core.Test.Fakers;

namespace Gatekeeper.Core.Test.Repositories;

[TestFixture]
public class UnitRepositoryTest : DbTest
{
    private UnitRepository _repository = null!;
    
    [SetUp]
    public void Setup()
    {
        _repository = new UnitRepository(GetConnectionFactory());
    }
    
    [Test]
    public async Task ShouldGetById()
    {
        var unit = new UnitFaker(false).Generate();
        
        var id = await _repository.SaveAsync(unit, CancellationToken.None);

        var unitFromDatabase = await _repository.GetByIdAsync(id, CancellationToken.None);

        unitFromDatabase.Should().NotBeNull();
        unitFromDatabase!.Identifier.Should().Be(unit.Identifier);
        unitFromDatabase.Id.Should().BeGreaterThan(0);
    }
    
    [Test]
    public async Task ShouldSaveUnit()
    {
        var unit = new UnitFaker(false).Generate();
        
        var existsBeforeCreation = await _repository.ExistsIdentifierAsync(unit.Identifier, CancellationToken.None);

        existsBeforeCreation.Should().BeFalse();

        await _repository.SaveAsync(unit, CancellationToken.None);

        var existsAfterCreation = await _repository.ExistsIdentifierAsync(unit.Identifier, CancellationToken.None);

        existsAfterCreation.Should().BeTrue();
    }
}