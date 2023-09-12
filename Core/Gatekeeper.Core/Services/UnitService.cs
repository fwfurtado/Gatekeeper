using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _repository;
    private readonly IValidator<RegisterUnitCommand> _registerUnitValidator;
    private readonly IValidator<RegisterResidentCommand> _registerResidentValidator;
    private readonly IMapper _mapper;

    public UnitService(
        IUnitRepository repository,
        IValidator<RegisterUnitCommand> registerUnitValidator,
        IValidator<RegisterResidentCommand> registerResidentValidator,
        IMapper mapper
    )
    {
        _repository = repository;
        _registerUnitValidator = registerUnitValidator;
        _registerResidentValidator = registerResidentValidator;
        _mapper = mapper;
    }

    public async Task<Unit> RegisterUnitAsync(RegisterUnitCommand command, CancellationToken cancellationToken)
    {
        await _registerUnitValidator.ValidateAndThrowAsync(command, cancellationToken);

        if (await _repository.ExistsIdentifierAsync(command.Identifier, cancellationToken))
        {
            var failure = new ValidationFailure("Identifier", "Identifier already exists");
            throw new ValidationException(new[] { failure });
        }
        
        var unit = _mapper.Map<Unit>(command);

        cancellationToken.ThrowIfCancellationRequested();

        await _repository.SaveAsync(unit, cancellationToken);

        return unit;
    }

    public Task<Unit?> GetUnitByIdAsync(long unitId, CancellationToken cancellationToken)
    {
        return _repository.GetByIdAsync(unitId, cancellationToken);
    }

    public async Task<Resident> RegisterResidentAsync(long unitId, RegisterResidentCommand command, CancellationToken cancellationToken)
    {

        var validationResult = await _registerResidentValidator.ValidateAsync(command, cancellationToken);
        
        var possibleUnit = await _repository.GetByIdAsync(unitId, cancellationToken) ;
        
        
        if (possibleUnit is null)
        {
            validationResult.Errors.Add(new ValidationFailure(nameof(unitId), "Unit not found"));
        }
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var unit = possibleUnit!;
        
        var resident = _mapper.Map<Resident>(command);
        
        unit.AssociateResident(resident);
        
        cancellationToken.ThrowIfCancellationRequested();
        
        await _repository.UpdateAsync(unit, cancellationToken);

        return resident;
    }
}