using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _repository;
    private readonly IValidator<RegisterTenantCommand> _validator;

    public TenantService(ITenantRepository repository, IValidator<RegisterTenantCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task RegisterTenantAsync(RegisterTenantCommand command, CancellationToken cancellationToken)
    {

        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var tenant = new Tenant(command.Name, command.Document);

        cancellationToken.ThrowIfCancellationRequested();

        await _repository.SaveAsync(tenant, cancellationToken);
    }
}
