using Microsoft.EntityFrameworkCore;
using Nero.Entities;

namespace Nero.Data;

public class NeroDbContext : DbContext
{
    public NeroDbContext(DbContextOptions<NeroDbContext> options)
        : base(options)
    {
    }

    public DbSet<Balance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Balance>();
    }

    public void SeedData()
    {
        if (!Balances.Any())
        {
            Balances.AddRange(
                Balance.Create("Some Account", 500)
            );
            SaveChanges();
        }
    }
}