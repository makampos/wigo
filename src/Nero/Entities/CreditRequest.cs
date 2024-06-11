namespace Nero.Entities;

public record CreditRequest(Guid UserId, string UserAccountBalanceNumber, decimal Amount);