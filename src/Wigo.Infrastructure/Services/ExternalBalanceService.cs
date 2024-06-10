using Flurl.Http;
using Wigo.Infrastructure.Interfaces;

namespace Wigo.Infrastructure.Services;

public class ExternalBalanceService : IExternalBalanceService
{
    private readonly string _balanceServiceBaseUrl;

    public ExternalBalanceService(string balanceServiceBaseUrl)
    {
        _balanceServiceBaseUrl = balanceServiceBaseUrl;
    }

    public async Task<decimal> GetBalanceAsync(string userAccountBalanceNumber)
    {
        try
        {
            var response = await $"{_balanceServiceBaseUrl}/balance/{userAccountBalanceNumber}".GetJsonAsync<BalanceResponse>();
            return response.Amount;
        }
        catch (FlurlHttpException ex)
        {
            // Handle exceptions and possibly log them
            throw new ApplicationException("Error fetching balance from external service.", ex);
        }
    }

    public async Task<bool> DebitBalanceAsync(string userAccountBalanceNumber, decimal amount)
    {
        try
        {
            var response = await $"{_balanceServiceBaseUrl}/debit"
                .PostJsonAsync(new { UserAccountBalanceNumber = userAccountBalanceNumber, Amount = amount })
                .ReceiveJson<bool>();
            return response; //Check if the request was successful
        }
        catch (FlurlHttpException ex)
        {
            // Handle exceptions and possibly log them
            throw new ApplicationException("Error debiting balance from external service.", ex);
        }
    }

    private record BalanceResponse
    {
        public string Name { get; init; }
        public decimal Amount { get; init; }
    }
}