using LifeOrganizer.Application.Features.GymSessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GymSessionsController : ControllerBase
{
    private readonly ISender _mediator;

    public GymSessionsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<GymSessionDto>>> Get([FromQuery] int take = 20)
        => Ok(await _mediator.Send(new GetGymSessionsQuery(take)));

    [HttpGet("active")]
    public async Task<ActionResult<GymSessionDto?>> GetActive()
        => Ok(await _mediator.Send(new GetActiveGymSessionQuery()));

    [HttpPost("start")]
    public async Task<ActionResult<Guid>> Start()
        => Ok(await _mediator.Send(new StartGymSessionCommand()));

    [HttpPost("{id:guid}/end")]
    public async Task<IActionResult> End(Guid id)
    {
        await _mediator.Send(new EndGymSessionCommand(id));
        return NoContent();
    }
}
