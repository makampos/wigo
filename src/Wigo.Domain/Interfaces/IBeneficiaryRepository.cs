using Wigo.Domain.Entities;

namespace Wigo.Domain.Interfaces;

public interface IBeneficiaryRepository
{
    Task<Guid> AddBeneficiaryAsync(Beneficiary beneficiary);
    Task<Beneficiary?> GetBeneficiaryByIdAsync(Guid beneficiaryId);
    Task<IEnumerable<Beneficiary>> GetBeneficiariesByUserIdAsync(Guid userId);
}