using Microsoft.AspNetCore.Mvc;
using Nero.Entities;
using Nero.Requests;
using Nero.Services;

namespace Nero.Controllers;

[Route("api/account-balance")]
[ApiController]
public class AccountBalanceController : ControllerBase
{
    private readonly IAccountBalanceService _accountBalanceService;
    private readonly ILogger<AccountBalanceController> _logger;

    public AccountBalanceController(IAccountBalanceService accountBalanceService, ILogger<AccountBalanceController> logger)
    {
        _accountBalanceService = accountBalanceService;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccountBalanceAsync([FromBody] CreateAccountBalanceRequest request)
    {
        _logger.LogInformation("Creating balance account for user {UserId}", request.UserId);

        var userAccountBalanceNumber = await _accountBalanceService.CreateAccountBalanceAsync(
            userId: request.UserId,
            name: request.Name);

        return Ok(userAccountBalanceNumber);
    }

    [HttpGet("{userId}/{userAccountBalanceNumber}")]
    public async Task<IActionResult> GetAccountBalanceAsync(Guid userId, string userAccountBalanceNumber)
    {
        _logger.LogInformation("Getting balance account for user {UserId}", userId);

        // Assuming that the user is authenticated and the user's balance account number is passed in the request
        // Further validation isn't in the scope of this example
        var balance = await _accountBalanceService.GetAccountBalanceAsync(userId, userAccountBalanceNumber);

        if (balance is null)
        {
            _logger.LogWarning("Balance account not found for user {UserId}", userId);
            return NotFound();
        }

        return Ok(balance.ToDTO());
    }

    [HttpPut("debit")]
    public async Task<IActionResult> DebitAccountBalanceAsync([FromBody] CreateDebitRequest request)
    {
        _logger.LogInformation("Debiting balance account for user {UserId}", request.UserId);

        var success = await _accountBalanceService.DebitAccountBalanceAsync(
            userId: request.UserId,
            userAccountBalanceNumber: request.UserAccountBalanceNumber,
            amount: request.Amount);

        if (!success)
        {
            _logger.LogWarning("Insufficient balance for user {UserId}", request.UserId);
            return BadRequest("Insufficient balance.");
        }

        return Ok(success);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> CreditAccountBalanceAsync([FromBody] CreateCreditRequest request)
    {
        _logger.LogInformation("Crediting balance account for user {UserId}", request.UserId);

        var success = await _accountBalanceService.CreditAccountBalanceAsync(
            userId: request.UserId,
            userAccountBalanceNumber: request.UserAccountBalanceNumber,
            amount: request.Amount);

        if (!success)
        {
            _logger.LogWarning("Failed to credit balance for user {UserId}", request.UserId);
            return BadRequest("Failed to credit balance.");
        }

        return Ok(success);
    }
}