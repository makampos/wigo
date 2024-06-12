using Microsoft.EntityFrameworkCore;
using Nero.Data;
using Nero.Entities;

namespace Nero.Services;

public class AccountBalanceService : IAccountBalanceService
{
    private readonly NeroDbContext _context;
    private readonly ILogger<AccountBalanceService> _logger;

    public AccountBalanceService(NeroDbContext context, ILogger<AccountBalanceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> CreateAccountBalanceAsync(Guid userId, string name)
    {
        var account = await _context.Balances
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (account.Any())
        {
            _logger.LogWarning("User {UserId} already has a balance account.", userId);
            // Ideally not throwing an exception here, but this is just a simple example
            // There are better ways to handle this, like returning a result object
            throw new Exception("User already has a balance account.");
        }

        var balance = AccountBalance.Create(userId, name);

        _context.Balances.Add(balance);

        await _context.SaveChangesAsync();

        return balance.UserAccountBalanceNumber;
    }

    public async Task<AccountBalance?> GetAccountBalanceAsync(Guid userId, string userAccountBalanceNumber)
    {
        return await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);
    }

    public async Task<bool> DebitAccountBalanceAsync(Guid userId, string userAccountBalanceNumber, decimal amount)
    {
        var balance = await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);

        if (balance == null || balance.Amount < amount)
        {
            _logger.LogCritical("Balance account not found or insufficient funds for user {UserId}", userId);
            return false;
        }

        balance = balance with { Amount = balance.Amount - amount };

        _context.Balances.Update(balance);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CreditAccountBalanceAsync(Guid userId, string userAccountBalanceNumber, decimal amount)
    {
        var balance = await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);

        if (balance == null)
        {
            _logger.LogCritical("Balance account not found for user {UserId}", userId);
            return false;
        }

        balance = balance with { Amount = balance.Amount + amount };

        _context.Balances.Update(balance);

        await _context.SaveChangesAsync();

        return true;
    }
}