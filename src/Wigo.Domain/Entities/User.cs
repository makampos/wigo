using System.Collections.ObjectModel;

namespace Wigo.Domain.Entities;

public record User
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public bool IsVerified { get; init; } = false;
    public virtual IEnumerable<Beneficiary> Beneficiaries { get; init; } = new Collection<Beneficiary>();

    public static User Create(string name)
    {
        return new User
        {
            Name = name
        };
    }
}