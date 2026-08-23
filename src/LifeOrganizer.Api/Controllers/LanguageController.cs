using LifeOrganizer.Application.Features.Language;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeOrganizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : ControllerBase
{
    private readonly ISender _mediator;

    public LanguageController(ISender mediator) => _mediator = mediator;

    [HttpGet("words")]
    public async Task<ActionResult<List<LanguageWordDto>>> GetWords()
        => Ok(await _mediator.Send(new GetLanguageWordsQuery()));

    [HttpPost("words")]
    public async Task<ActionResult<Guid>> CreateWord(CreateLanguageWordCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetWords), null, id);
    }

    [HttpDelete("words/{id:guid}")]
    public async Task<IActionResult> DeleteWord(Guid id)
    {
        await _mediator.Send(new DeleteLanguageWordCommand(id));
        return NoContent();
    }

    [HttpGet("progress")]
    public async Task<ActionResult<LanguageProgressDto>> GetProgress()
        => Ok(await _mediator.Send(new GetLanguageProgressQuery()));

    [HttpPut("progress")]
    public async Task<IActionResult> UpdateProgress(UpdateLanguageProgressCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}
