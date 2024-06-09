using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;

namespace Wigo.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Beneficiary> Beneficiaries { get; set; }
    public DbSet<TopUpOption> TopUpOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Beneficiaries)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Beneficiary>()
            .HasOne(b => b.User)
            .WithMany(u => u.Beneficiaries)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Beneficiary>()
            .Property(b => b.Nickname)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<TopUpOption>().HasData(
            TopUpOption.Create(amount: 5m),
            TopUpOption.Create(amount: 10m),
            TopUpOption.Create(amount: 20m),
            TopUpOption.Create(amount: 30m),
            TopUpOption.Create(amount: 50m),
            TopUpOption.Create(amount: 75m),
            TopUpOption.Create(amount: 100m)
        );
    }
}