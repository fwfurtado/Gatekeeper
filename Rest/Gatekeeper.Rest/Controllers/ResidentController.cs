using AutoMapper;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Services;
using Gatekeeper.Rest.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Controllers;

[ApiController]
[Route("residents")]
// [Authorize] //TODO: Uncomment this line when authentication is implemented in Frontend.Gatekeeper.Admin
public class ResidentController : ControllerBase
{

    private readonly IResidentService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<ResidentController> _logger;

    public ResidentController(IResidentService service, IMapper mapper, ILogger<ResidentController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreatResident([FromBody] RegisterResidentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register a new resident with params {Params}", request);

        try
        {
            var command = _mapper.Map<RegisterResidentCommand>(request);
            var resident = await _service.RegisterResidentAsync(command, cancellationToken);
            _logger.LogInformation("Resident registered with success");
            return CreatedAtAction(nameof(ShowResident), new { residentId = resident.Id }, null);
        }
        catch (InvalidOperationException invEx)
        {
            return Conflict(invEx.Message);
        }
        catch (ValidationException ex) 
        {
            return BadRequest(ex.Message);
        }        
    }

    [HttpGet]
    public async Task<IActionResult> ListResidents(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all residents");

        var residents = await _service.GetAllResidents(cancellationToken);

        _logger.LogInformation("Residents found");

        var response = residents.Select(u => _mapper.Map<ResidentResponse>(u));

        return Ok(response);
    }

    [HttpGet("{residentId:long}")]
    public async Task<IActionResult> ShowResident(long residentId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get resident with id {ResidentId}", residentId);

        var resident = await _service.GetResidentByIdAsync(residentId, cancellationToken);

        if (resident is null)
        {
            _logger.LogInformation("Resident with id {ResidentId} not found", residentId);
            return NotFound();
        }

        _logger.LogInformation("Resident with id {ResidentId} found", residentId);

        var response = _mapper.Map<ResidentResponse>(resident);

        return Ok(response);
    }

    [HttpDelete("{residentId:long}")]
    public async Task<IActionResult> DeleteResident(long residentId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete resident with id {ResidentId}", residentId);

        await _service.DeleteResident(residentId, cancellationToken);
                
        _logger.LogInformation("Resident with id {ResidentId} was deleted", residentId);

        return NoContent();
    }
}