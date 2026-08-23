using LifeOrganizer.Application.Features.ExerciseLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseLogsController : ControllerBase
{
    private readonly ISender _mediator;

    public ExerciseLogsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ExerciseLogDto>>> GetHistory([FromQuery] Guid exerciseId)
        => Ok(await _mediator.Send(new GetExerciseHistoryQuery(exerciseId)));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateExerciseLogCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHistory), new { exerciseId = command.ExerciseId }, id);
    }
}
