using FluentAssertions;
using FluentValidation.TestHelper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Test.Fakers;
using Gatekeeper.Core.Validations;
using Moq;

namespace Gatekeeper.Core.Test.Validations;

public class RegisterUnitCommandValidatorTest
{
    [Test]
    public async Task ShouldBeValid()
    {
        var command = new RegisterUnitCommandFaker().Generate();
        
        var policy = new Mock<IUnitIdentifierDuplicatedPolicy>();
        
        policy.Setup(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        
        var validator = new RegisterUnitCommandValidator(policy.Object);
        
        var result = await validator.TestValidateAsync(command);
        
        result.ShouldNotHaveAnyValidationErrors();
        
        policy.Verify(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task ShouldBeInvalidWhenIdentifierIsInvalid()
    {
        var command = new RegisterUnitCommand(string.Empty);
        
        var policy = new Mock<IUnitIdentifierDuplicatedPolicy>();
        
        policy.Setup(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        
        var validator = new RegisterUnitCommandValidator(policy.Object);
        
        var result = await validator.TestValidateAsync(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Identifier);
        
        policy.Verify(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task ShouldBeInvalidWhenIdentifierAlreadyExists()
    {
        var command = new RegisterUnitCommandFaker().Generate();
        
        var policy = new Mock<IUnitIdentifierDuplicatedPolicy>();
        
        policy.Setup(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        
        var validator = new RegisterUnitCommandValidator(policy.Object);
        
        var result = await validator.TestValidateAsync(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Identifier);
        
        policy.Verify(p => p.IsValidAsync(command.Identifier, It.IsAny<CancellationToken>()), Times.Once);
    }
}