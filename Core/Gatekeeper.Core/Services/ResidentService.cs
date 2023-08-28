using AutoMapper;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class ResidentService : IResidenttService
{
    private readonly IResidentRepository _repository;
    private readonly IValidator<RegisterResidentCommand> _validator;
    private readonly IMapper _mapper;

    public ResidentService(IResidentRepository repository, IValidator<RegisterResidentCommand> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task RegisterTenantAsync(RegisterResidentCommand command, CancellationToken cancellationToken)
    {

        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var tenant = _mapper.Map<Resident>(command);

        cancellationToken.ThrowIfCancellationRequested();

        await _repository.SaveAsync(tenant, cancellationToken);
    }
}
