using MediatR;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Abstractions;
using Wigo.Service.Commands;

namespace Wigo.Service.Handlers;

public class AddBeneficiaryCommandHandler : IRequestHandler<AddBeneficiaryCommand, ServiceResult<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    public AddBeneficiaryCommandHandler(IUserRepository userRepository, IBeneficiaryRepository beneficiaryRepository)
    {
        _userRepository = userRepository;
        _beneficiaryRepository = beneficiaryRepository;
    }

    public async Task<ServiceResult<Guid>> Handle(AddBeneficiaryCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIAsync(request.UserId);

        if (user is null)
        {
            return ServiceResult<Guid>.FailureResult("User not found.");
        }

        var beneficiaries = await _beneficiaryRepository.GetBeneficiariesByUserIdAsync(userId: request.UserId);

        // Business role: A user cannot have more than 5 beneficiaries
        if (beneficiaries.Count() >= 5)
        {
            return ServiceResult<Guid>.FailureResult("User cannot have more than 5 beneficiaries.");
        }

        var beneficiary = Beneficiary.Create(
            userId: request.UserId,
            nickname: request.Nickname,
            phoneNumber: request.PhoneNumber);

        try
        {
            var id = await _beneficiaryRepository.AddBeneficiaryAsync(beneficiary);
            return ServiceResult<Guid>.SuccessResult(id);
        }
        catch (Exception ex)
        {
            return ServiceResult<Guid>.FailureResult($"Failed to create beneficiary: {ex.Message}");
        }
    }
}