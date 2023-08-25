using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _repository;

    public TenantService(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task RegisterTenantAsync(RegisterTenantCommand command, CancellationToken cancellationToken)
    {

        if (await _repository.ExistsDocumentAsync(command.Document, cancellationToken))              
        {
            throw new ArgumentException("Document already exists");
        }     

        var tenant = new Tenant(command.Name, command.Document);

        cancellationToken.ThrowIfCancellationRequested();

        await _repository.SaveAsync(tenant, cancellationToken);
    }
}
