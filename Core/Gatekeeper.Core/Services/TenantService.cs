using AutoMapper;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _repository;
    private readonly IValidator<RegisterTenantCommand> _validator;
    private readonly IMapper _mapper;

    public TenantService(ITenantRepository repository, IValidator<RegisterTenantCommand> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task RegisterTenantAsync(RegisterTenantCommand command, CancellationToken cancellationToken)
    {

        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var tenant = _mapper.Map<Tenant>(command);

        cancellationToken.ThrowIfCancellationRequested();

        await _repository.SaveAsync(tenant, cancellationToken);
    }
}
