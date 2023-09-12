﻿using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gatekeeper.Rest.Controllers;

[ApiController]
[Route("residents")]
public class ResidentController : ControllerBase
{

    private readonly IResidentService _service;
    private readonly ILogger<ResidentController> _logger;

    public ResidentController(IResidentService service, ILogger<ResidentController> logger)
    {
        _service = service;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreatResident([FromBody] RegisterResidentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register a new resident with params {Params}", command);

        try
        {
            var resident = await _service.RegisterResidentAsync(command, cancellationToken);
            _logger.LogInformation("Resident registered with success");
            return CreatedAtAction(nameof(ShowResident), resident.Id);
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

        return Ok(resident);
    }


}