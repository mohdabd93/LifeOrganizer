using LifeOrganizer.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator) => _mediator = mediator;

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
        => Ok(await _mediator.Send(new GetMeQuery()));

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("me/password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
