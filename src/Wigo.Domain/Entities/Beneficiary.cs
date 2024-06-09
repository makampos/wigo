namespace Wigo.Domain.Entities;

public record Beneficiary
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Nickname { get; init; }
    public required string PhoneNumber { get; init; }
    public User User { get; init; }

    public static Beneficiary Create(Guid id, Guid userId, string nickname, string phoneNumber)
    {
        return new Beneficiary
        {
            Id = id,
            UserId = userId,
            Nickname = nickname,
            PhoneNumber = phoneNumber
        };
    }
}