using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Rest.Controllers;

namespace Gatekeeper.Rest.Factories;

public class NewOccupationCommandFactory {
    
    private readonly IUnitRepository _repository;
    private readonly IMapper _mapper;

    public NewOccupationCommandFactory(IUnitRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NewOccupationCommand> FactoryBy(UnitOccupationApplicationRequest request, CancellationToken cancellationToken)
    {
        var unit = await _repository.GetByIdentifier(request.UnitIdentifier, cancellationToken) ??
            throw new InvalidOperationException("Unit does not exists");
        
        var people = request.People.Select(p => _mapper.Map<PersonalInfo>(p)).ToList();
        
        return new NewOccupationCommand(unit, people);
    }
}