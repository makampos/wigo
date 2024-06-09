using MediatR;
using Wigo.Service.Abstractions;

namespace Wigo.Service.Commands;

public record AddBeneficiaryCommand(
    Guid UserId,
    string Nickname,
    string PhoneNumber) : IRequest<ServiceResult<Guid>>;