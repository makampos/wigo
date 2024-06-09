using Wigo.Domain.Entities;

namespace Wigo.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<Guid> AddUserAsync(User user);
    Task<User?> GetUserByIAsync(Guid userId);
}