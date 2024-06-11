namespace Nero.Entities;

public record DebitRequest(Guid UserId, string UserAccountBalanceNumber, decimal Amount);