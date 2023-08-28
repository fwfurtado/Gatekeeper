using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations.Validations;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;

namespace Gatekeeper.Core.Validations;

public class RegisterResidentCommandValidator: AbstractValidator<RegisterResidentCommand>
{ 
    
    public RegisterResidentCommandValidator(IResidentRepository repository, ICpfPolicy policy)
    {
        RuleFor(r => r.Name).Length(1, 100);
        RuleFor(r => r.Document)
            .Length(11)
            .Cpf(policy)
            .CustomAsync(async (value, context, cancellationToken) =>
            {
                var exists = await repository.ExistsDocumentAsync(value, cancellationToken);

                if (exists)
                {
                    context.AddFailure("Document already exists");
                }
            });
    }
}