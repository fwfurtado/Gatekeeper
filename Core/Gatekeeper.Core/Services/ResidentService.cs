using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;

namespace Gatekeeper.Core.Services;

public class ResidentService : IResidentService
{
    private readonly IResidentRepository _repository;
    private readonly IValidator<RegisterResidentCommand> _registerResidentValidator;
    private readonly IMapper _mapper;

    public ResidentService(
        IResidentRepository repository,
        IValidator<RegisterResidentCommand> registerResidentValidator,
        IMapper mapper
    )
    {
        _repository = repository;
        _registerResidentValidator = registerResidentValidator;
        _mapper = mapper;
    }

    public async Task<Resident> RegisterResidentAsync(RegisterResidentCommand command, CancellationToken cancellationToken)
    {
        await _registerResidentValidator.ValidateAndThrowAsync(command, cancellationToken);
        var duplicatedDocument = await _repository.ExistsDocumentAsync(command.Document.Number, cancellationToken);

        if (duplicatedDocument)
        {
            throw new InvalidOperationException("Document already exists");
        }

        var resident = _mapper.Map<Resident>(command);
        
        cancellationToken.ThrowIfCancellationRequested();

        var id = await _repository.SaveAsync(resident, cancellationToken);

        resident.Id = id;
        
        return resident;
    }
    
    public Task<Resident?> GetResidentByIdAsync(long residentId, CancellationToken cancellationToken)
    {
        return _repository.GetByIdAsync(residentId, cancellationToken);
    }

    public async Task<IEnumerable<Resident>> GetAllResidents(CancellationToken cancellationToken)
    {
        return await _repository.GetAll(cancellationToken);
    }

    public async Task DeleteResident(long residentId, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIdAsync(residentId, cancellationToken);
    }

}
