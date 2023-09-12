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
    
    private readonly IMapper _mapper;

    public UnitService(
        IUnitRepository repository,
        IValidator<RegisterUnitCommand> registerUnitValidator,
        IMapper mapper
    )
    {
        _repository = repository;
        _registerUnitValidator = registerUnitValidator;
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
}