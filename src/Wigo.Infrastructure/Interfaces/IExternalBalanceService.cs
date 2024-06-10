namespace Wigo.Infrastructure.Interfaces;

public interface IExternalBalanceService
{
    Task<decimal> GetBalanceAsync(string userAccountBalanceNumber);
    Task<bool> DebitBalanceAsync(string userAccountBalanceNumber, decimal amount);
}