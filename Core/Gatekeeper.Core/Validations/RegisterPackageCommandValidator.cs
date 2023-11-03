using FluentValidation;
using Gatekeeper.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatekeeper.Core.Validations;

public class RegisterPackageCommandValidator : AbstractValidator<RegisterPackageCommand>
{
    public RegisterPackageCommandValidator()
    {
        RuleFor(command => command.Description)
            .NotEmpty()
            .MaximumLength(300);
    }
}
