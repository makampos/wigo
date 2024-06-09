namespace Wigo.Domain.Entities;

public record User
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public bool IsVerified { get; init; } = false;
    public required ICollection<Beneficiary> Beneficiaries { get; init; } = new List<Beneficiary>();

    public static User Create(Guid id, string name, ICollection<Beneficiary> beneficiaries)
    {
        return new User
        {
            Id = id,
            Name = name,
            Beneficiaries = beneficiaries
        };
    }
}