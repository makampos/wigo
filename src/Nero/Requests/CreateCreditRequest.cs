namespace Nero.Requests;

public record CreateCreditRequest(Guid UserId, string UserAccountBalanceNumber, decimal Amount);