using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wigo.Service.Commands;

namespace Wigo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopUpTransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TopUpTransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddTopUpTransactionAsync([FromBody] AddTopUpTransactionCommand command)
    {
        var serviceResult = await _mediator.Send(command);
        if (!serviceResult.Success)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }

        return Ok(serviceResult.Data);
    }
}