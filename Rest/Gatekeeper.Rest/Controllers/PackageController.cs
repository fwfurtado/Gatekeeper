using AutoMapper;
using Gatekeeper.Core.Services;
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