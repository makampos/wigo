using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Data;

namespace Wigo.Infrastructure.Repositories;

public class TopUpRepository : ITopUpRepository
{
    private readonly ApplicationDbContext _context;

    public TopUpRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TopUpOption>> GetTopUpOptionsAsync()
    {
        return await _context.TopUpOptions.ToListAsync();
    }
}