using Microsoft.EntityFrameworkCore;
using Nero.Entities;

namespace Nero.Data;

public class NeroDbContext : DbContext
{
    public NeroDbContext(DbContextOptions<NeroDbContext> options)
        : base(options)
    {
    }

    public DbSet<AccountBalance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountBalance>();
    }
}