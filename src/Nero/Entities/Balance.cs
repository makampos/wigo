using Nero.Helpers;

namespace Nero.Entities;

public record Balance
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string UserAccountBalanceNumber { get; init; }
    public required decimal Amount { get; init; }

    public static Balance Create(string name, decimal amount)
    {
        return new Balance
        {
            Amount = amount,
            Name = name,
            UserAccountBalanceNumber = UserAccountBalanceNumberGenerator.GenerateUserAccountBalanceNumber(),
        };
    }
}