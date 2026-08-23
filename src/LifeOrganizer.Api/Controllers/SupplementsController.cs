using LifeOrganizer.Application.Features.Supplements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplementsController : ControllerBase
{
    private readonly ISender _mediator;

    public SupplementsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<SupplementDto>>> Get()
        => Ok(await _mediator.Send(new GetSupplementsQuery()));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateSupplementCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id:guid}/reminder")]
    public async Task<IActionResult> ToggleReminder(Guid id, [FromBody] bool reminderEnabled)
    {
        await _mediator.Send(new ToggleSupplementReminderCommand(id, reminderEnabled));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSupplementCommand(id));
        return NoContent();
    }
}
