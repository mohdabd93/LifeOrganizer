using LifeOrganizer.Application.Features.GymSplits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GymSplitsController : ControllerBase
{
    private readonly ISender _mediator;

    public GymSplitsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<GymSplitDto>>> Get()
        => Ok(await _mediator.Send(new GetGymSplitsQuery()));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateGymSplitCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteGymSplitCommand(id));
        return NoContent();
    }
}
