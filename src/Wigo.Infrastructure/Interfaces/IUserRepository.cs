using Wigo.Domain.Entities;

namespace Wigo.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
    Task<User> GetUserByIAsync(Guid id);
    Task<User> GetUserByIdWithBeneficiariesIncludeAsync(Guid id);
}