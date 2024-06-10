namespace Wigo.Domain.Entities;

public record TopUpTransaction
{
    public Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid BeneficiaryId { get; init; }
    public required decimal Amount { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal Charge { get; init; }

    public virtual User User { get; init; }
    public virtual Beneficiary Beneficiary { get; init; }

    public static TopUpTransaction Create(Guid userId, Guid beneficiaryId, decimal amount)
    {
        return new TopUpTransaction
        {
            UserId = userId,
            BeneficiaryId = beneficiaryId,
            Amount = amount,
            CreatedAt = DateTime.Now,
            Charge = 1.0m // fee
        };
    }
}