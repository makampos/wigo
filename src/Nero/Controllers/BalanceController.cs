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

    [HttpPost("create")]
    public async Task<IActionResult> CreateBalanceAccountAsync([FromBody] CreateBalanceRequest request)
    {
        var userAccountBalanceNumber = await _balanceService.CreateBalanceAccountAsync(
            userId: request.UserId,
            name: request.Name);

        return Ok(userAccountBalanceNumber);
    }

    [HttpGet("{userId}/{userAccountBalanceNumber}")]
    public async Task<IActionResult> GetBalance(Guid userId, string userAccountBalanceNumber)
    {
        // Assuming that the user is authenticated and the user's account balance number is passed in the request
        // Further validation isn't in the scope of this example
        var balance = await _balanceService.GetBalanceAccountAsync(userId, userAccountBalanceNumber);
        if (balance is null)
        {
            return NotFound();
        }

        return Ok(balance.ToDTO());
    }

    [HttpPost("debit")]
    public async Task<IActionResult> DebitBalance([FromBody] DebitRequest request)
    {
        var success = await _balanceService.DebitBalanceAccountAsync(
            userId: request.UserId,
            userAccountBalanceNumber: request.UserAccountBalanceNumber,
            amount: request.Amount);

        if (!success)
        {
            return BadRequest("Insufficient balance.");
        }

        return Ok(success);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> CreditBalance([FromBody] CreditRequest request)
    {
        var success = await _balanceService.CreditBalanceAccountAsync(
            userId: request.UserId,
            userAccountBalanceNumber: request.UserAccountBalanceNumber,
            amount: request.Amount);

        if (!success)
        {
            return BadRequest("Failed to credit balance.");
        }

        return Ok(success);
    }
}