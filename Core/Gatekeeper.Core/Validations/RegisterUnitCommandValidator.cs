using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Validations;

public class RegisterUnitCommandValidator : AbstractValidator<RegisterUnitCommand>
{
    public RegisterUnitCommandValidator(IUnitRepository repository)
    {
        RuleFor(command => command.Identifier)
            .NotEmpty()
            .MaximumLength(100)
            .CustomAsync(async (identifier, context, cancellationToken) =>
            {
                var exists = await repository.ExistsIdentifierAsync(identifier, cancellationToken);

                if (exists)
                {
                    context.AddFailure("Identifier already exists");
                }
            });
    }
}