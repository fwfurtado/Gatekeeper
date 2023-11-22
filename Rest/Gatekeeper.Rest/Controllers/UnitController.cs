using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Controllers;

[ApiController]
[Route("units")]
[Authorize]
public class UnitController : ControllerBase
{
    private readonly IUnitService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<UnitController> _logger;

    public UnitController(IUnitService service, IMapper mapper, ILogger<UnitController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreateUnit([FromBody] RegisterUnitCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register a  new unit with params {Params}", command);

        var unit = await _service.RegisterUnitAsync(command, cancellationToken);

        _logger.LogInformation("Unit registered with success");

        return CreatedAtAction(nameof(ShowUnit), new { unitId = unit.Id }, null);
    }
    
    [HttpGet]
    public async Task<IActionResult> ListUnits(
        CancellationToken cancellationToken,
        [FromQuery] PageRequest? pageRequest = null
        )
    {
        _logger.LogInformation("Get all units");
        
        var page = pageRequest ?? new PageRequest(0, 10);
        
        var units = await _service.GetAllUnits(page, cancellationToken);

        _logger.LogInformation("Units found");

        var response = units.Select(u => _mapper.Map<UnitResponse>(u));

        return Ok(response);
    }

    [HttpGet("{unitId:long}")]
    public async Task<IActionResult> ShowUnit(long unitId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get unit with id {UnitId}", unitId);

        var unit = await _service.GetUnitByIdAsync(unitId, cancellationToken);

        if (unit is null)
        {
            _logger.LogInformation("Unit with id {UnitId} not found", unitId);
            return NotFound();
        }

        _logger.LogInformation("Unit with id {UnitId} found", unitId);

        var response = _mapper.Map<UnitResponse>(unit);

        return Ok(response);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterUnitByIdentifier(CancellationToken cancellationToken, [FromQuery] string? identifier = null)
    {
        IEnumerable<Unit> units;
        
        if (identifier is null) 
        {
            units = await _service.GetTenFirstUnitsAsync(cancellationToken);
        }
        else 
        {
            units = await _service.FilterByIdentifierAsync(identifier, cancellationToken);
        }             

        var response = units.Select(_mapper.Map<UnitResponse>);

        return Ok(response);
    }
}