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

    public async Task<Balance> GetBalanceAsync(string userAccountBalanceNumber)
    {
        return await _context.Balances.FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber);
    }

    public async Task<bool> DebitBalanceAsync(string userAccountBalanceNumber, decimal amount)
    {
        var balance = await _context.Balances.FirstOrDefaultAsync(b => b.UserAccountBalanceNumber == userAccountBalanceNumber);
        if (balance == null || balance.Amount < amount)
        {
            return false;
        }

        balance = balance with { Amount = balance.Amount - amount }; // Update the balance

        _context.Balances.Update(balance);
        await _context.SaveChangesAsync();

        return true;
    }
}