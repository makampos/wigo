using MediatR;
using Wigo.Service.Abstractions;

namespace Wigo.Service.Commands;

public record AddTopUpTransactionCommand(
    Guid UserId,
    Guid BeneficiaryId,
    string UserAccountBalanceNumber,
    decimal Amount) : IRequest<ServiceResult<Guid>>;