using MediatR;
using Wigo.Service.Abstractions;

namespace Wigo.Service.Commands;

public record AddTopUpTransactionCommand(
    Guid UserId,
    Guid BeneficiaryId,
    string UserAccountBalanceNumber,
    decimal Amount) : IRequest<ServiceResult<Guid>>
{
    public AddTopUpTransactionCommand ApplyCharge()
    {
        const decimal charge = 1m;
        return this with { Amount = this.Amount + charge };
    }
}