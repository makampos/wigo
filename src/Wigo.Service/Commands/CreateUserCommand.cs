using MediatR;
using Wigo.Service.Abstractions;

namespace Wigo.Service.Commands;

public record AddUserCommand(string Name) : IRequest<ServiceResult<Guid>>;