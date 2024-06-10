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
    public DbSet<TopUpTransaction> TopUpTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // A User can have many Beneficiaries and many TopUpTransactions.
        modelBuilder.Entity<User>()
            .HasMany(u => u.Beneficiaries)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.TopUpTransactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Beneficiary>()
            .HasOne(b => b.User)
            .WithMany(u => u.Beneficiaries)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Beneficiary>()
            .HasMany(b => b.TopUpTransactions)
            .WithOne(t => t.Beneficiary)
            .HasForeignKey(t => t.BeneficiaryId);

        modelBuilder.Entity<Beneficiary>()
            .Property(b => b.Nickname)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<TopUpOption>().HasData(
            TopUpOption.Create(amount: TopUpOptionsEnum.AED5),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED10),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED20),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED30),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED50),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED75),
            TopUpOption.Create(amount: TopUpOptionsEnum.AED100)
        );
    }
}