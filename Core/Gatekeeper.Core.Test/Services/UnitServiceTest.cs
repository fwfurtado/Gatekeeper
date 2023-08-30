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

public class UnitServiceTest
{
    private Mock<IUnitRepository> _repositoryMock = null!;
    private IMapper _mapper = null!;
    private RegisterUnitCommandValidator _unitValidator = null!;
    private RegisterResidentCommandValidator _residentValidator = null!;

    [SetUp]
    public void BeforeEach()
    {
        _repositoryMock = new Mock<IUnitRepository>();
        
        var configuration = AutoMapperConfiguration.Configure();
        _mapper = configuration.CreateMapper();

        var unitRegisterPolicy = new UnitIdentifierDuplicatedPolicy(_repositoryMock.Object);
        _unitValidator = new RegisterUnitCommandValidator(unitRegisterPolicy);
        _residentValidator = new RegisterResidentCommandValidator(new CpfPolicy());
    }
    
    [Test]
    public void ShouldFailWhenCommandIsInvalid()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);

        var command = new RegisterUnitCommand(string.Empty);
        
        Assert.ThrowsAsync<ValidationException>(() => service.RegisterUnitAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenIdentifierAlreadyExists()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        Assert.ThrowsAsync<ValidationException>(() => service.RegisterUnitAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenCancellationWasRequired()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        cancellationTokenSource.Cancel();
        
        Assert.ThrowsAsync<OperationCanceledException>(() => service.RegisterUnitAsync(command, token));
    }

    [Test]
    public async Task ShouldCreateAnEmptyUnit()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        var unit = await service.RegisterUnitAsync(command, token);
        
        unit.Should().NotBeNull();
        unit.Identifier.Should().Be(command.Identifier);
        unit.Residents.Should().BeEmpty();
    }
    
    [Test]
    public async Task ShouldAddResidentToUnit()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        var unit = new UnitFaker().Generate();
        
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(unit);

        var unitId = new Faker().Random.Long(min: 1);
        
        var residentCommand = new RegisterResidentCommandFaker().Generate();
        
        var resident = await service.RegisterResidentAsync(unitId, residentCommand, token);
        
        resident.Should().NotBeNull();
        resident.Name.Should().Be(residentCommand.Name);
        resident.Document.Should().Be(residentCommand.Document);
        
        unit.Residents.Should().HaveCount(1);
        unit.Residents.Should().ContainSingle(r => r.Name == resident.Name && r.Document == resident.Document);
        
        _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(unit, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void ShouldFailToAddResidentToUnitWhenUnitDoesNotExists()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _residentValidator, _mapper);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var unitId = new Faker().Random.Long(min: 1);
        
        var residentCommand = new RegisterResidentCommandFaker().Generate();
        
        Invoking(() => service.RegisterResidentAsync(unitId, residentCommand, token))
            .Should().ThrowAsync<ValidationException>();
        
        _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void ShouldFailToAddResidentToUnitWhenCommandIsInvalid()
    {
        var residentValidator = new InlineValidator<RegisterResidentCommand>();

        residentValidator
            .RuleFor(c => c.Document)
            .Must(_ => false);
        
        residentValidator
            .RuleFor(c => c.Document)
            .Must(_ => false);
        
        var service = new UnitService(_repositoryMock.Object, _unitValidator, residentValidator, _mapper);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        var unit = new UnitFaker().Generate();
        
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(unit);

        var unitId = new Faker().Random.Long(min: 1);
        
        var residentCommand = new RegisterResidentCommandFaker().Generate();
        
        Invoking(() => service.RegisterResidentAsync(unitId, residentCommand, token))
            .Should().ThrowAsync<ValidationException>();
        
        _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}