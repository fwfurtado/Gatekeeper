using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.ValueObjects;
using Gatekeeper.Rest.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Controllers;

[ApiController]
[Route("packages")]
[Authorize]
public class PackageController : ControllerBase
{
    private readonly IPackageService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<PackageController> _logger;

    public PackageController(IPackageService service, IMapper mapper, ILogger<PackageController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost]
    public async Task<IActionResult> CreatePackage([FromBody] RegisterPackageCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Register a new package with params {Params}", command);

        var package = await _service.RegisterPackageAsync(command, cancellationToken);

        _logger.LogInformation("Package registered with success");

        return CreatedAtAction(nameof(ShowPackage), new { packageId = package.Id }, null);
    }
    
    [HttpGet]
    public async Task<IActionResult> ListPackages(
        CancellationToken cancellationToken,
        [FromQuery] PageRequest? pageRequest = null
        )
    {
        _logger.LogInformation("Get all packages");
        
        var page = pageRequest ?? new PageRequest(0, 10);
        
        var packages = await _service.GetAllPackages(page, cancellationToken);

        _logger.LogInformation("Packages found");

        var response = packages.Select(u => _mapper.Map<PackageResponse>(u));

        return Ok(response);
    }

    [HttpGet("{packageId:long}")]
    public async Task<IActionResult> ShowPackage(long packageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get package with id {PackageId}", packageId);

        var package = await _service.GetPackageByIdAsync(packageId, cancellationToken);

        if (package is null)
        {
            _logger.LogInformation("Package with id {PackageId} not found", packageId);
            return NotFound();
        }

        _logger.LogInformation("Package with id {PackageId} found", packageId);

        var response = _mapper.Map<PackageResponse>(package);

        return Ok(response);
    }

    [HttpDelete("{packageId:long}")]
    public async Task<IActionResult> DeletePackage(long packageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete package with id {PackageId}", packageId);

        await _service.DeletePackage(packageId, cancellationToken);

        _logger.LogInformation("Package with id {PackageId} was deleted", packageId);

        return NoContent();
    }

    [HttpPatch("{packageId:long}/delivered")]
    public async Task<IActionResult> UpdateDeliveredStatus(long packageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update package with id {PackageId} with status Delivered", packageId);

        await _service.UpdateStatusDeliveredAsync(packageId, cancellationToken);

        _logger.LogInformation("Package with id {PackageId} was updated to Delivered", packageId);

        return NoContent();
    }

    [HttpPatch("{packageId:long}/rejected")]
    public async Task<IActionResult> UpdateRejectedStatus(long packageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update package with id {PackageId} with status Rejected", packageId);

        await _service.UpdateStatusRejectedAsync(packageId, cancellationToken);

        _logger.LogInformation("Package with id {PackageId} was updated to Rejected", packageId);

        return NoContent();
    }
}