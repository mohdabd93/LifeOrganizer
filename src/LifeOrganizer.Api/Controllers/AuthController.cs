using LifeOrganizer.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register(RegisterCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login(LoginCommand command)
        => Ok(await _mediator.Send(command));
}
