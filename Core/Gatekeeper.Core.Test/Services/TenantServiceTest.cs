using Bogus;
using Bogus.Extensions.Brazil;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Services;
using Moq;

namespace Gatekeeper.Core.Test.Services;

public class TenantServiceTest
{
    private Mock<ITenantRepository> _repositoryMock;
    private CancellationTokenSource _tokenSource;
    private readonly Faker<RegisterTenantCommand> _commandFaker = new Faker<RegisterTenantCommand>()
            .CustomInstantiator(f => new RegisterTenantCommand(f.Person.FullName, f.Person.Cpf()));


    [SetUp]
    public void BeforeEach()
    {
        _tokenSource = new CancellationTokenSource();
        _repositoryMock = new Mock<ITenantRepository>();
    }

    [Test]
    public void ShouldFailWhenDocumentAlreadyExists()
    {
       
        _repositoryMock.Setup(r =>r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new TenantService(_repositoryMock.Object);

        var command = _commandFaker.Generate();

        Assert.ThrowsAsync<ArgumentException>(() => service.RegisterTenantAsync(command, _tokenSource.Token));

        _repositoryMock.Verify(r => r.ExistsDocumentAsync(command.Document, _tokenSource.Token), Times.Once);
        _repositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ShouldRegisterANewTenant()
    {

        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new TenantService(_repositoryMock.Object);

        var command = _commandFaker.Generate();

        await service.RegisterTenantAsync(command, _tokenSource.Token);

        _repositoryMock.Verify(r => r.ExistsDocumentAsync(command.Document, _tokenSource.Token), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Tenant>(), _tokenSource.Token), Times.Once);

    }
}
