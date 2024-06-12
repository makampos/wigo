using Nero.Entities;

namespace Nero.Services;

public interface IAccountBalanceService
{
    Task<string> CreateAccountBalanceAsync(Guid userId, string name);
    Task<AccountBalance?> GetAccountBalanceAsync(Guid userId, string userAccountBalanceNumber);
    Task<bool> DebitAccountBalanceAsync(Guid userId, string userAccountBalanceNumber, decimal amount);
    Task<bool> CreditAccountBalanceAsync(Guid userId, string userAccountBalanceNumber, decimal amount);
}