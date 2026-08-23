using LifeOrganizer.Application.Features.ScheduleBlocks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleBlocksController : ControllerBase
{
    private readonly ISender _mediator;

    public ScheduleBlocksController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ScheduleBlockDto>>> Get()
        => Ok(await _mediator.Send(new GetScheduleBlocksQuery()));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateScheduleBlockCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateScheduleBlockCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteScheduleBlockCommand(id));
        return NoContent();
    }

    public record ToggleCompletionRequest(DateOnly Date, bool IsDone);

    [HttpPost("{id:guid}/toggle-completion")]
    public async Task<IActionResult> ToggleCompletion(Guid id, ToggleCompletionRequest request)
    {
        await _mediator.Send(new ToggleBlockCompletionCommand(id, request.Date, request.IsDone));
        return NoContent();
    }
}
