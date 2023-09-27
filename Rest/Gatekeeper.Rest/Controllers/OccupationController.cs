using Gatekeeper.Core.Exceptions;
using Gatekeeper.Core.Services;
using Gatekeeper.Rest.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Gatekeeper.Rest.Controllers;

[ApiController]
[Route("occupations")]
public class OccupationController : ControllerBase
{
    private readonly IOccupationService _service;
    private readonly NewOccupationCommandFactory _factory;

    public OccupationController(IOccupationService service, NewOccupationCommandFactory factory)
    {
        _service = service;
        _factory = factory;
    }

    [HttpPost("application")]
    public async Task<IActionResult> ApplyForOccupation(
        UnitOccupationApplicationRequest applicationRequest,
        CancellationToken cancellationToken
    )
    {
        var command = await _factory.FactoryBy(applicationRequest, cancellationToken);

        await _service.RequestOccupationAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet("requests/{id}")]
    public async Task<IActionResult> GetRequestById(long id, CancellationToken cancellationToken)
    {
        var request = await _service.GetById(id, cancellationToken);

        if (request is null) return NotFound();

        return Ok(request);
    }

    [HttpPut("requests/{id}/approve")]
    public async Task<IActionResult> ApproveRequestById(long id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.ApproveRequestAsync(id, cancellationToken);

            return Accepted();
        }
        catch (OccupationRequestNotFouncException)
        {
            return NotFound();
        }
    }

    [HttpPut("requests/{id}/reject")]
    public async Task<IActionResult> RejectRequestById(long id, RejectOccupationRequest reject,
        CancellationToken cancellationToken)
    {
        try
        {
            await _service.RejectRequestAsync(id, reject.Reason, cancellationToken);

            return Accepted();
        }
        catch (OccupationRequestNotFouncException)
        {
            return NotFound();
        }
    }
}

public class RejectOccupationRequest
{
    public string Reason { get; set; }
}

public class UnitOccupationApplicationRequest
{
    public string UnitIdentifier { get; set; }
    public List<PersonInfoRequest> People { get; init; }
}

public class PersonInfoRequest
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Document { get; init; }
}