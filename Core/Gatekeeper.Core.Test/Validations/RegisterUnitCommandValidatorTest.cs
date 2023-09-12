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
        
        var validator = new RegisterUnitCommandValidator();
        
        var result = await validator.TestValidateAsync(command);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test]
    public async Task ShouldBeInvalidWhenIdentifierIsInvalid()
    {
        var command = new RegisterUnitCommand(string.Empty);
        
        
        var validator = new RegisterUnitCommandValidator();
        
        var result = await validator.TestValidateAsync(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Identifier);
    }
}