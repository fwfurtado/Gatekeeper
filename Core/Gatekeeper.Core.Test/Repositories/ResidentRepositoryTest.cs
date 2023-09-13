using FluentAssertions;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Test.Configurations;
using Gatekeeper.Core.Test.Fakers;

namespace Gatekeeper.Core.Test.Repositories;

[TestFixture]
public class ResidentRepositoryTest : DbTest
{
    private ResidentRepository _repository = null!;
    
    [SetUp]
    public void Setup()
    {
        _repository = new ResidentRepository(GetConnectionFactory());
    }
    
    [Test]
    public async Task ShouldGetById()
    {
        var resident = new ResidentFaker().Generate();
        
        var id = await _repository.SaveAsync(resident, CancellationToken.None);

        var residentFromDatabase = await _repository.GetByIdAsync(id, CancellationToken.None);

        residentFromDatabase.Should().NotBeNull();
        residentFromDatabase!.Document.Should().Be(resident.Document);
        residentFromDatabase.Name.Should().Be(resident.Name);
        residentFromDatabase.Id.Should().BeGreaterThan(0);
    }
    
    [Test]
    public async Task ShouldSaveUnit()
    {
        var resident = new ResidentFaker().Generate();
        
        var existsBeforeCreation = await _repository.ExistsDocumentAsync(resident.Document, CancellationToken.None);

        existsBeforeCreation.Should().BeFalse();

        await _repository.SaveAsync(resident, CancellationToken.None);

        var existsAfterCreation = await _repository.ExistsDocumentAsync(resident.Document, CancellationToken.None);

        existsAfterCreation.Should().BeTrue();
    }
}