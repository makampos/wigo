namespace Wigo.Service.DTOs;

public record BeneficiaryDto(
    Guid BeneficiaryId,
    string Nickname,
    string PhoneNumber)
{
    public static BeneficiaryDto Create(Guid beneficiaryId, string nickname, string phoneNumber)
    {
        return new BeneficiaryDto(beneficiaryId, nickname, phoneNumber);
    }
}