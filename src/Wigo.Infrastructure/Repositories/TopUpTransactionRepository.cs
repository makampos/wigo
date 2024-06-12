using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Data;

namespace Wigo.Infrastructure.Repositories;

public class TopUpTransactionRepository : ITopUpTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TopUpTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddTopUpTransactionAsync(TopUpTransaction topUpTransaction)
    {
        _context.TopUpTransactions.Add(topUpTransaction);
        await _context.SaveChangesAsync();
        return topUpTransaction.Id;
    }

    public async Task<TopUpTransaction?> GetTopUpTransactionByIdAsync(Guid topUpTransactionId)
    {
        return await _context.TopUpTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == topUpTransactionId);
    }

    public async Task<IEnumerable<TopUpTransaction>> GetTopUpTransactionsByUserIdAsync(Guid userId)
    {
        //TODO: Improve
        return await _context.TopUpTransactions
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    ///   Get the total amount of top-up transactions for a beneficiary in the current month
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="beneficiaryId"></param>
    /// <returns></returns>
    public async Task<decimal> GetMonthlyTotalForBeneficiary(Guid userId, Guid beneficiaryId)
    {
        // Get the start of the current month
        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var endDate = startDate.AddMonths(1);

        return await _context.TopUpTransactions
            .Where(t => t.UserId == userId && t.BeneficiaryId == beneficiaryId && t.CreatedAt >= startDate && t.CreatedAt < endDate)
            .SumAsync(t => t.Amount);
    }

    /// <summary>
    ///  Get the total amount of top-up transactions for a user in the current month
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<decimal> GetMonthlyTotalForUser(Guid userId)
    {
        // Get the start of the current month
        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var endDate = startDate.AddMonths(1);

        return await _context.TopUpTransactions
            .Where(t => t.UserId == userId && t.CreatedAt >= startDate && t.CreatedAt < endDate)
            .SumAsync(t => t.Amount);
    }
}