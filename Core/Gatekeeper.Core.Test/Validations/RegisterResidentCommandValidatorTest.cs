using FluentAssertions;
using FluentValidation.TestHelper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Test.Fakers;
using Gatekeeper.Core.Validations;
using Moq;

namespace Gatekeeper.Core.Test.Validations;

public class RegisterResidentCommandValidatorTest
{
    [Test]
    public void ShouldBeValid()
    {
        var policyMock = new Mock<ICpfPolicy>();
        
        policyMock.Setup(p => p.IsValid(It.IsAny<string>())).Returns(true);
        
        var command = new RegisterResidentCommandFaker().Generate();
        var validator = new RegisterResidentCommandValidator(policyMock.Object);
        
        var result = validator.TestValidate(command);
        
        result.ShouldNotHaveAnyValidationErrors();
        
        policyMock.Verify(p => p.IsValid(It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public void ShouldBeInvalidWhenNameIsInvalid()
    {
        var policyMock = new Mock<ICpfPolicy>();
        
        policyMock.Setup(p => p.IsValid(It.IsAny<string>())).Returns(true);
        
        var command = new RegisterResidentCommand(string.Empty, new CpfFaker().Generate());
        
        var validator = new RegisterResidentCommandValidator(policyMock.Object);
        
        var result = validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Name);
        
        policyMock.Verify(p => p.IsValid(It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public void ShouldBeInvalidWhenDocumentIsInvalid()
    {
        var policyMock = new Mock<ICpfPolicy>();
        
        policyMock.Setup(p => p.IsValid(It.IsAny<string>())).Returns(false);
        
        var command = new RegisterResidentCommandFaker().Generate();
        
        var validator = new RegisterResidentCommandValidator(policyMock.Object);
        
        var result = validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(c => c.Document.Number);
        
        policyMock.Verify(p => p.IsValid(It.IsAny<string>()), Times.Once);
    }
}