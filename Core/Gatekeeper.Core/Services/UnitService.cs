using AutoMapper;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class UnitService: IUnitService
{
    private readonly IUnitRepository _repository;
    private readonly IValidator<RegisterUnitCommand> _validator;
    private readonly IMapper _mapper;

    public UnitService(IUnitRepository repository, IValidator<RegisterUnitCommand> validator, IMapper mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken)
    {

        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var unit = _mapper.Map<Unit>(command);

        cancellationToken.ThrowIfCancellationRequested();
        
        await _repository.SaveAsync(unit, cancellationToken);

        return unit;
    } 
        
}