namespace Wigo.Domain.Entities;

public record TopUpOption
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public static TopUpOption Create(decimal amount)
    {
        return new TopUpOption
        {
            Id = Guid.NewGuid(),
            Amount = amount
        };
    }
}