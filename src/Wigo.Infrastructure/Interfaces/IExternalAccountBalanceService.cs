namespace Wigo.Infrastructure.Interfaces;

public interface IExternalAccountBalanceService
{
    Task<decimal> GetAccountBalanceAsync(Guid userId, string userAccountBalanceNumber);
    Task<bool> DebitAccountBalanceAsync(Guid userId, string userAccountBalanceNumber, decimal amount);
}