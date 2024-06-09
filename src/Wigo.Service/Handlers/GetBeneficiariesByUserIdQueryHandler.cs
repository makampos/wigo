using MediatR;
using Wigo.Domain.Interfaces;
using Wigo.Service.Abstractions;
using Wigo.Service.DTOs;
using Wigo.Service.Mappers;
using Wigo.Service.Queries;

namespace Wigo.Service.Handlers;

public class GetBeneficiariesByUserIdQueryHandler : IRequestHandler<GetBeneficiariesByUserIdQuery, ServiceResult<IEnumerable<BeneficiaryDto>>>
{
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    public GetBeneficiariesByUserIdQueryHandler(IBeneficiaryRepository beneficiaryRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
    }

    public async Task<ServiceResult<IEnumerable<BeneficiaryDto>>> Handle(GetBeneficiariesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByUserIdAsync(request.UserId);

        var beneficiaryDto = beneficiaries.Select(b => BeneficiaryMapper.MapToBeneficiaryDto(b));

        return ServiceResult<IEnumerable<BeneficiaryDto>>.SuccessResult(beneficiaryDto);
    }
}
