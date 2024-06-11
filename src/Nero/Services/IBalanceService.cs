using Nero.Entities;

namespace Nero.Services;

public interface IBalanceService
{
    Task<string> CreateBalanceAccountAsync(Guid userId, string name);
    Task<Balance?> GetBalanceAccountAsync(Guid userId, string userAccountBalanceNumber);
    Task<bool> DebitBalanceAccountAsync(Guid userId, string userAccountBalanceNumber, decimal amount);
    Task<bool> CreditBalanceAccountAsync(Guid userId, string userAccountBalanceNumber, decimal amount);
}