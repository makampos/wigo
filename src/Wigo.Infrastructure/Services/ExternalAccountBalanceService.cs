using Flurl.Http;
using Wigo.Infrastructure.Interfaces;

namespace Wigo.Infrastructure.Services;

public class ExternalAccountBalanceService : IExternalAccountBalanceService
{
    private readonly string _externalAccountBalanceServiceBaseUrl;

    public ExternalAccountBalanceService(string externalAccountBalanceServiceBaseUrl)
    {
        _externalAccountBalanceServiceBaseUrl = externalAccountBalanceServiceBaseUrl;
    }

    public async Task<decimal> GetAccountBalanceAsync(string userAccountBalanceNumber)
    {
        try
        {
            var response = await $"{_externalAccountBalanceServiceBaseUrl}/account-balance/{userAccountBalanceNumber}".GetJsonAsync<AccountBalanceResponse>();
            return response.Amount;
        }
        catch (FlurlHttpException ex)
        {
            // Handle exceptions and possibly log them
            throw new ApplicationException("Error fetching balance from external service.", ex);
        }
    }

    public async Task<bool> DebitAccountBalanceAsync(string userAccountBalanceNumber, decimal amount)
    {
        try
        {
            var response = await $"{_externalAccountBalanceServiceBaseUrl}/account-balance/debit"
                .PostJsonAsync(new CreateDebitRequest(userAccountBalanceNumber, amount))
                .ReceiveJson<bool>();
            return response; //Check if the request was successful
        }
        catch (FlurlHttpException ex)
        {
            // Handle exceptions and possibly log them
            throw new ApplicationException("Error debiting balance from external service.", ex);
        }
    }

    public record CreateDebitRequest(string UserAccountBalanceNumber, decimal Amount);

    private record AccountBalanceResponse
    {
        public string Name { get; init; }
        public decimal Amount { get; init; }
    }
}