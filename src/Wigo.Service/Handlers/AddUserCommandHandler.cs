using MediatR;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Abstractions;
using Wigo.Service.Commands;

namespace Wigo.Service.Handlers;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ServiceResult<Guid>>
{
    private readonly IUserRepository _userRepository;

    public AddUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            name: request.Name);

        try
        {
            var id = await _userRepository.AddUserAsync(user);
            return ServiceResult<Guid>.SuccessResult(id);
        }
        catch (Exception ex)
        {
            return ServiceResult<Guid>.FailureResult($"Failed to create user: {ex.Message}");
        }
    }
}