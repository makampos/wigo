using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Data;
using Wigo.Infrastructure.Interfaces;

namespace Wigo.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByIAsync(Guid userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> GetUserByIdWithBeneficiariesIncludeAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.Beneficiaries)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}