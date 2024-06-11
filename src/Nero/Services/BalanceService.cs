using Microsoft.EntityFrameworkCore;
using Nero.Data;
using Nero.Entities;

namespace Nero.Services;

public class BalanceService : IBalanceService
{
    private readonly NeroDbContext _context;

    public BalanceService(NeroDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateBalanceAccountAsync(Guid userId, string name)
    {
        var account = await _context.Balances
            .Where(x => x.UserId == userId)
            .ToListAsync();

        if (account.Any())
        {
            // Ideally not throwing an exception here, but this is just a simple example
            // There are better ways to handle this, like returning a result object
            throw new Exception("User already has a balance account.");
        }

        var balance = Balance.Create(userId, name);

        _context.Balances.Add(balance);

        await _context.SaveChangesAsync();

        return balance.UserAccountBalanceNumber;
    }

    public async Task<Balance?> GetBalanceAccountAsync(Guid userId, string userAccountBalanceNumber)
    {
        return await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);
    }

    public async Task<bool> DebitBalanceAccountAsync(Guid userId, string userAccountBalanceNumber, decimal amount)
    {
        var balance = await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);

        if (balance == null || balance.Amount < amount)
        {
            return false;
        }

        balance = balance with { Amount = balance.Amount - amount }; // Update the balance

        _context.Balances.Update(balance);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CreditBalanceAccountAsync(Guid userId, string userAccountBalanceNumber, decimal amount)
    {
        var balance = await _context.Balances
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber && b.UserId == userId);

        if (balance == null)
        {
            return false;
        }

        balance = balance with { Amount = balance.Amount + amount }; // Update the balance

        _context.Balances.Update(balance);

        await _context.SaveChangesAsync();

        return true;
    }
}