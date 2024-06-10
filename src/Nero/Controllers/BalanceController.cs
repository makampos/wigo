using Microsoft.AspNetCore.Mvc;
using Nero.Entities;
using Nero.Services;

namespace Nero.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalanceController : ControllerBase
{
    private readonly IBalanceService _balanceService;

    public BalanceController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet("{userAccountBalanceNumber}")]
    public async Task<IActionResult> GetBalance(string userAccountBalanceNumber)
    {
        // Assuming that the user is authenticated and the user's account balance number is passed in the request
        // Further validation isn't in the scope of this example

        var balance = await _balanceService.GetBalanceAsync(userAccountBalanceNumber);
        if (balance is null)
        {
            return NotFound();
        }

        return Ok(balance.ToDTO());
    }

    [HttpPost("debit")]
    public async Task<IActionResult> DebitBalance([FromBody] DebitRequest request)
    {
        var success = await _balanceService.DebitBalanceAsync(
            userAccountBalanceNumber: request.UserAccountBalanceNumber,
            amount: request.Amount);

        if (!success)
        {
            return BadRequest("Insufficient balance.");
        }

        return Ok();
    }
}