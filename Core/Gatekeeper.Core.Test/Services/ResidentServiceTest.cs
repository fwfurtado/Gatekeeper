using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Test.Fakers;
using Gatekeeper.Core.Validations;
using Moq;
using ValidationException = FluentValidation.ValidationException;
using static FluentAssertions.FluentActions;

namespace Gatekeeper.Core.Test.Services;

public class ResidentServiceTest
{
    private Mock<IResidentRepository> _repositoryMock = null!;
    private IMapper _mapper = null!;
    private RegisterResidentCommandValidator _residentValidator = null!;

    [SetUp]
    public void BeforeEach()
    {
        _repositoryMock = new Mock<IResidentRepository>();

        var configuration = AutoMapperConfiguration.Configure();
        _mapper = configuration.CreateMapper();

        _residentValidator = new RegisterResidentCommandValidator(new CpfSpecification());
    }

    [Test]
    public void ShouldFailWhenCommandIsInvalid()
    {
        var service = new ResidentService(_repositoryMock.Object, _residentValidator, _mapper);
        var faker = new CpfFaker();
        var command = new RegisterResidentCommand(string.Empty, faker.Generate());

        Assert.ThrowsAsync<ValidationException>(() => service.RegisterResidentAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenDocumentAlreadyExists()
    {
        var service = new ResidentService(_repositoryMock.Object, _residentValidator, _mapper);

        var command = new RegisterResidentCommandFaker().Generate();

        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterResidentAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenCancellationWasRequired()
    {
        var service = new ResidentService(_repositoryMock.Object, _residentValidator, _mapper);

        var command = new RegisterResidentCommandFaker().Generate();

        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var cancellationTokenSource = new CancellationTokenSource();

        var token = cancellationTokenSource.Token;

        cancellationTokenSource.Cancel();

        Assert.ThrowsAsync<OperationCanceledException>(() => service.RegisterResidentAsync(command, token));
    }

    [Test]
    public async Task ShouldCreateAResident()
    {
        var service = new ResidentService(_repositoryMock.Object, _residentValidator, _mapper);

        var command = new RegisterResidentCommandFaker().Generate();

        _repositoryMock.Setup(r => r.ExistsDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var cancellationTokenSource = new CancellationTokenSource();

        var token = cancellationTokenSource.Token;

        var resident = await service.RegisterResidentAsync(command, token);

        resident.Should().NotBeNull();
        resident.Name.Should().Be(command.Name);
        resident.Document.Should().Be(command.Document);
    }
}
