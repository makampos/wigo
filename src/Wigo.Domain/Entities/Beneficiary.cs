namespace Wigo.Domain.Entities;

public record Beneficiary
{
    public Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Nickname { get; init; }
    public required string PhoneNumber { get; init; }
    public virtual User User { get; init; }

    public static Beneficiary Create(Guid userId, string nickname, string phoneNumber)
    {
        return new Beneficiary
        {
            UserId = userId,
            Nickname = nickname,
            PhoneNumber = phoneNumber
        };
    }
}