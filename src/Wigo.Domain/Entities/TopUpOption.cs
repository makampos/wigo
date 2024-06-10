namespace Wigo.Domain.Entities;

public record TopUpOption
{
    public Guid Id { get; init; }
    public TopUpOptionsEnum Amount { get; init; }

    public static TopUpOption Create(TopUpOptionsEnum amount)
    {
        return new TopUpOption
        {
            Id = Guid.NewGuid(),
            Amount = amount
        };
    }
}