using MediatR;
using Wigo.Service.Abstractions;
using Wigo.Service.DTOs;

namespace Wigo.Service.Queries;

public record GetBeneficiariesByUserIdQuery(Guid UserId) : IRequest<ServiceResult<IEnumerable<BeneficiaryDto>>>;