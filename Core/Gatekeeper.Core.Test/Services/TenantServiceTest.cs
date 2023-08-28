using AutoMapper;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Validations;
using Moq;
using ValidationException = FluentValidation.ValidationException;

namespace Gatekeeper.Core.Test.Services;

public class TenantServiceTest
{
    private Mock<ITenantRepository> _repositoryMock = null!;
    private CancellationTokenSource _tokenSource = null!;
    private IValidator<RegisterTenantCommand> _validator = null!;
    private TenantService _service = null!;

    private readonly Faker<RegisterTenantCommand> _commandFaker = new Faker<RegisterTenantCommand>()
        .CustomInstantiator(f => new RegisterTenantCommand(f.Person.FullName, f.Person.Cpf(false)));

    [SetUp]
    public void BeforeEach()
    {
        _tokenSource = new CancellationTokenSource();
        _repositoryMock = new Mock<ITenantRepository>();

        var cpfValidator = new CpfPolicy();
        _validator = new RegisterTenantCommandValidator(_repositoryMock.Object, cpfValidator);
        var config = AutoMapperConfiguration.Configure();
        var mapper = config.CreateMapper();

        _service = new TenantService(_repositoryMock.Object, _validator, mapper);
    }

    [Test]
    public void ShouldFailWhenDocumentAlreadyExists()
    {
        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = _commandFaker.Generate();

        Assert.ThrowsAsync<ValidationException>(() => _service.RegisterTenantAsync(command, _tokenSource.Token));

        _repositoryMock.Verify(r => r.ExistsDocumentAsync(command.Document, _tokenSource.Token), Times.Once);
        _repositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task ShouldRegisterANewTenant()
    {
        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var command = _commandFaker.Generate();

        await _service.RegisterTenantAsync(command, _tokenSource.Token);

        _repositoryMock.Verify(r => r.ExistsDocumentAsync(command.Document, _tokenSource.Token), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Tenant>(), _tokenSource.Token), Times.Once);
    }
}