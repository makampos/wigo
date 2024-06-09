using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wigo.Service.Commands;

namespace Wigo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddUserAsync([FromBody] AddUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }
}