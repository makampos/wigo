using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Data;

namespace Wigo.Infrastructure.Repositories;

public class BeneficiaryRepository : IBeneficiaryRepository
{
    private readonly ApplicationDbContext _context;

    public BeneficiaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddBeneficiaryAsync(Beneficiary beneficiary)
    {
        _context.Beneficiaries.Add(beneficiary);
        await _context.SaveChangesAsync();
        return beneficiary.Id;
    }

    public async Task<IEnumerable<Beneficiary>> GetBeneficiariesByUserIdAsync(Guid userId)
    {
        return await _context.Beneficiaries
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }
}