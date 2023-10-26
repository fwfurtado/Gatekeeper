using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Repositories;
using Npgsql.Internal.TypeHandlers.GeometricHandlers;

namespace Gatekeeper.Core.Services;

internal class ImportCsvService : IImportCsvService
{
    record Person(string Name, string Document);
    record Unit(string Identity);
    private readonly Dictionary<Unit, HashSet<Person>> _data = new();
    private readonly IUnitService _unitService;
    private readonly IOccupationService _occupationService;
    private readonly IUnitRepository _unitRepository;


    public ImportCsvService(IUnitService unitService, IOccupationService occupationService, IUnitRepository unitRepository)
    {
        _unitService = unitService;
        _occupationService = occupationService;
        _unitRepository = unitRepository;
    }

    public async Task ImportCsv(string filePath, CancellationToken cancellationToken)
    {
        foreach(var line in File.ReadLines(filePath))
        {
            var (person, unit) = ParseLine(line);

           var found = _data.TryGetValue(unit, out var data);

            if(found)
            {
                data.Add(person);
            }
            else
            {
                var data = new HashSet<Person> {person};
                _data.Add(unit, data);
            }
        }

        foreach(var (unit, people) in _data)
        {
            var unitCommand = new RegisterUnitCommand(unit.Identity);
            try
            {
                await _unitService.RegisterUnitAsync(unitCommand, cancellationToken);
            }
            catch (ValidationException ex) 
            {
                var alreadyExist = ex.Errors.Any(e => e.PropertyName == "Identifier" && e.ErrorMessage == "Identifier already exists");
                if (!alreadyExist) continue;
            }
            var dbUnit = await _unitRepository.GetByIdentifier(unit.Identity, cancellationToken);
            var targetUnit = new TargetUnit 
            { 
                Identifier = unit.Identity,
                UnitId = dbUnit!.Id
            };
            var peopleRequest = people.Select(p => new PersonalInfo
            {
                Name = p.Name,
                Document = p.Document,
                Email = "",
                Phone = ""
            }).ToList();
            var occupationCommand = new NewOccupationCommand(targetUnit, peopleRequest);

            var requestId = await _occupationService.RequestOccupationAsync(occupationCommand, cancellationToken);

            await _occupationService.ApproveRequestAsync(requestId, cancellationToken);
        }
    }


    private static (Person, Unit) ParseLine (string line)
    {
        var parts = line.Split(',');
        var person = new Person(parts[0], parts[1]);
        var unit = new Unit(parts[2]);
        return (person, unit);
    }

}
