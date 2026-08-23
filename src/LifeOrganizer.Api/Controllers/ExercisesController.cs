using LifeOrganizer.Application.Features.Exercises;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ISender _mediator;

    public ExercisesController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ExerciseDto>>> GetBySplit([FromQuery] Guid gymSplitId)
        => Ok(await _mediator.Send(new GetExercisesBySplitQuery(gymSplitId)));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateExerciseCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBySplit), new { gymSplitId = command.GymSplitId }, id);
    }

    [HttpPut("{id:guid}/target")]
    public async Task<IActionResult> UpdateTarget(Guid id, [FromBody] decimal? nextTargetWeightKg)
    {
        await _mediator.Send(new UpdateExerciseTargetCommand(id, nextTargetWeightKg));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExerciseCommand(id));
        return NoContent();
    }
}
