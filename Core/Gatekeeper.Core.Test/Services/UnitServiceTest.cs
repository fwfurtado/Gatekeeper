using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Test.Fakers;
using Gatekeeper.Core.Validations;
using Moq;

namespace Gatekeeper.Core.Test.Services;

public class UnitServiceTest
{
    private Mock<IUnitRepository> _repositoryMock = null!;
    private IMapper _mapper = null!;
    private RegisterUnitCommandValidator _unitValidator = null!;
    

    [SetUp]
    public void BeforeEach()
    {
        _repositoryMock = new Mock<IUnitRepository>();
        
        var configuration = AutoMapperConfiguration.Configure();
        _mapper = configuration.CreateMapper();
        
        _unitValidator = new RegisterUnitCommandValidator();
    }
    
    [Test]
    public void ShouldFailWhenCommandIsInvalid()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _mapper);

        var command = new RegisterUnitCommand(string.Empty);
        
        Assert.ThrowsAsync<ValidationException>(() => service.RegisterUnitAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenIdentifierAlreadyExists()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        Assert.ThrowsAsync<ValidationException>(() => service.RegisterUnitAsync(command, CancellationToken.None));
    }

    [Test]
    public void ShouldFailWhenCancellationWasRequired()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        cancellationTokenSource.Cancel();
        
        Assert.ThrowsAsync<OperationCanceledException>(() => service.RegisterUnitAsync(command, token));
    }

    [Test]
    public async Task ShouldCreateAnUnit()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _mapper);

        var command = new RegisterUnitCommandFaker().Generate();
        
        _repositoryMock.Setup(r => r.ExistsIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        var unit = await service.RegisterUnitAsync(command, token);
        
        unit.Should().NotBeNull();
        unit.Identifier.Should().Be(command.Identifier);
    }
    
    [Test]
    public async Task ShouldReturnNullWhenUnitDoesNotExist()
    {
        var service = new UnitService(_repositoryMock.Object, _unitValidator, _mapper);

        var cancellationTokenSource = new CancellationTokenSource();
        
        var token = cancellationTokenSource.Token;
        
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Unit?) null);
        
        var unit = await service.GetUnitByIdAsync(1, token);
        
        unit.Should().BeNull();
    }
}