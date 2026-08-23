using LifeOrganizer.Application.Features.Activities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ISender _mediator;

    public ActivitiesController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> Get([FromQuery] DateOnly? fromDate)
        => Ok(await _mediator.Send(new GetActivitiesQuery(fromDate)));

     [HttpPost]
    public async Task<ActionResult<ActivityMutationResult>> Create(CreateActivityCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ActivityMutationResult>> Update(Guid id, UpdateActivityCommand command)
    {
        if (id != command.Id) return BadRequest();
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteActivityCommand(id));
        return NoContent();
    }
}
