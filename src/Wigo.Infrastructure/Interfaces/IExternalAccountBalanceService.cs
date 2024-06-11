namespace Wigo.Infrastructure.Interfaces;

public interface IExternalAccountBalanceService
{
    Task<decimal> GetAccountBalanceAsync(string userAccountBalanceNumber);
    Task<bool> DebitAccountBalanceAsync(string userAccountBalanceNumber, decimal amount);
}