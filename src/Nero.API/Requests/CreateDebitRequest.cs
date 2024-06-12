namespace Nero.Requests;

public record CreateDebitRequest(Guid UserId, string UserAccountBalanceNumber, decimal Amount);