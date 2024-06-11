using Nero.Helpers;

namespace Nero.Entities;

public record AccountBalance
{
    public Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required string UserAccountBalanceNumber { get; init; }
    public decimal Amount { get; init; }

    public static AccountBalance Create(Guid userId, string name)
    {
        return new AccountBalance
        {
            UserId = userId,
            Name = name,
            UserAccountBalanceNumber = UserAccountBalanceNumberGenerator.GenerateUserAccountBalanceNumber(),
        };
    }
}