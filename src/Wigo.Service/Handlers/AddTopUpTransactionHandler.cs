using MediatR;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Abstractions;
using Wigo.Service.Commands;

namespace Wigo.Service.Handlers;

public class AddTopUpTransactionHandler : IRequestHandler<AddTopUpTransactionCommand, ServiceResult<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly ITopUpTransactionRepository _topUpTransactionRepository;
    private readonly IExternalAccountBalanceService _externalAccountBalanceService;

    public AddTopUpTransactionHandler(
        IUserRepository userRepository,
        IBeneficiaryRepository beneficiaryRepository,
        ITopUpTransactionRepository topUpTransactionRepository,
        IExternalAccountBalanceService externalAccountBalanceService)
    {
        _userRepository = userRepository;
        _beneficiaryRepository = beneficiaryRepository;
        _topUpTransactionRepository = topUpTransactionRepository;
        _externalAccountBalanceService = externalAccountBalanceService;
    }

    public async Task<ServiceResult<Guid>> Handle(AddTopUpTransactionCommand command, CancellationToken cancellationToken)
    {
        command = command.ApplyCharge();

        var user = await _userRepository.GetUserByIAsync(command.UserId);
        if (user is null)
        {
            return ServiceResult<Guid>.FailureResult("User not found.");
        }

        // ensure the beneficiary belongs to the user
        var beneficiary = await _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId);
        if (beneficiary is null || beneficiary.UserId != command.UserId)
        {
            return ServiceResult<Guid>.FailureResult("Beneficiary not found or does not belong to the user.");
        }

        // Check user's balance
        var balance = await _externalAccountBalanceService.GetAccountBalanceAsync(command.UserAccountBalanceNumber);
        var totalAmount = command.Amount;
        if (totalAmount > balance)
        {
            return ServiceResult<Guid>.FailureResult("Insufficient balance.");
        }

        // Calculate monthly top-up totals
        var monthlyTotalForBeneficiary = await _topUpTransactionRepository.GetMonthlyTotalForBeneficiary(command.UserId, command.BeneficiaryId);
        var monthlyTotalForUser = await _topUpTransactionRepository.GetMonthlyTotalForUser(command.UserId);

        // Check if the user has exceeded the monthly top-up limit
        var maxPerBeneficiary = user.IsVerified ? 500m : 1000m;
        if (monthlyTotalForBeneficiary + command.Amount > maxPerBeneficiary)
        {
            return ServiceResult<Guid>.FailureResult($"Exceeded monthly top-up limit per beneficiary of AED {maxPerBeneficiary}.");
        }

        if (monthlyTotalForUser + command.Amount > 3000m)
        {
            return ServiceResult<Guid>.FailureResult("Exceeded monthly top-up limit for all beneficiaries of AED 3000.");
        }

        // Debit user's balance through external service
        var debitResult = await _externalAccountBalanceService.DebitAccountBalanceAsync(command.UserAccountBalanceNumber, totalAmount);
        if (!debitResult)
        {
            return ServiceResult<Guid>.FailureResult("Failed to debit balance.");
        }

        // Create top-up transaction
        var topUpTransaction = TopUpTransaction.Create(
            userId: command.UserId,
            beneficiaryId: command.BeneficiaryId,
            amount: command.Amount);

        // Add top-up transaction
        var topUpTransactionId = await _topUpTransactionRepository.AddTopUpTransactionAsync(topUpTransaction);
        return ServiceResult<Guid>.SuccessResult(topUpTransactionId);
    }
}