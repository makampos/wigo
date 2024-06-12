using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wigo.Service.Commands;
using Wigo.Service.Queries;

namespace Wigo.API.Controllers;

[ApiController]
[Route("api/beneficiary")]
public class BeneficiaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public BeneficiaryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddBeneficiaryAsync([FromBody] AddBeneficiaryCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{userId}/beneficiaries")]
    public async Task<IActionResult> GetBeneficiariesByUserIdAsync(Guid userId)
    {
        var query = new GetBeneficiariesByUserIdQuery(UserId: userId);
        var result = await _mediator.Send(query);

        if (result.Success)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.ErrorMessage);
    }
}