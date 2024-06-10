using Nero.Entities;

namespace Nero.Services;

public interface IBalanceService
{
    Task<Balance?> GetBalanceAsync(string userAccountBalanceNumber);
    Task<bool> DebitBalanceAsync(string userAccountBalanceNumber, decimal amount);
}