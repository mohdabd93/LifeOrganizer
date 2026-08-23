using LifeOrganizer.Application.Features.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly ISender _mediator;

    public SettingsController(ISender mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<AppSettingsDto>> Get()
        => Ok(await _mediator.Send(new GetSettingsQuery()));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateSettingsCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
