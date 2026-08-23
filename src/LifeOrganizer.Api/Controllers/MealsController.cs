using LifeOrganizer.Application.Features.Meals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    private readonly ISender _mediator;

    public MealsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<MealDto>>> Get([FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetMealsQuery(date)));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateMealCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { date = command.Date }, id);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMealCommand(id));
        return NoContent();
    }
}
