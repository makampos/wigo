using Wigo.Domain.Entities;
using Wigo.Service.DTOs;

namespace Wigo.Service.Mappers;

public static class BeneficiaryMapper
{
    public static BeneficiaryDto MapToBeneficiaryDto(Beneficiary beneficiary)
    {
        return BeneficiaryDto.Create(
            beneficiaryId: beneficiary.Id,
            nickname: beneficiary.Nickname,
            phoneNumber: beneficiary.PhoneNumber);
    }
}