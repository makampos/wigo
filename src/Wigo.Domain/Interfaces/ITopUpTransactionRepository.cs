using Wigo.Domain.Entities;

namespace Wigo.Domain.Interfaces;

public interface ITopUpTransactionRepository
{
    Task<Guid> AddTopUpTransactionAsync(TopUpTransaction topUpTransaction);
    Task<TopUpTransaction?> GetTopUpTransactionByIdAsync(Guid topUpTransactionId);
    Task<IEnumerable<TopUpTransaction>> GetTopUpTransactionsByUserIdAsync(Guid userId);
    Task<decimal> GetMonthlyTotalForBeneficiary(Guid userId, Guid beneficiaryId);
    Task<decimal> GetMonthlyTotalForUser(Guid userId);
}