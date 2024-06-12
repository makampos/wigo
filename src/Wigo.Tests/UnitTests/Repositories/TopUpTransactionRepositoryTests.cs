using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Data;
using Wigo.Infrastructure.Repositories;

namespace Wigo.Tests.UnitTests.Repositories;

public class TopUpTransactionRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public TopUpTransactionRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "WigoTestDb")
            .Options;
    }

    [Fact]
    public async Task GetMonthlyTotalForBeneficiary_ShouldReturnCorrectTotal()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();

        await using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.TopUpTransactions.AddRange(
                TopUpTransaction.Create(userId, beneficiaryId, 100m),
                TopUpTransaction.Create(userId, beneficiaryId, 200m),
                TopUpTransaction.Create(userId, Guid.NewGuid(), 300m) with { CreatedAt = DateTime.Now.AddMonths(-1)} // previous month
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var repository = new TopUpTransactionRepository(context);

            // Act
            var total = await repository.GetMonthlyTotalForBeneficiary(userId, beneficiaryId);

            // Assert
            total.Should().Be(300m);
        }
    }

    [Fact]
    public async Task GetMonthlyTotalForUser_ShouldReturnCorrectTotal()
    {
        // Arrange
        var userId = Guid.NewGuid();

        await using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            context.TopUpTransactions.AddRange(
                TopUpTransaction.Create(userId, Guid.NewGuid(), 100m),
                TopUpTransaction.Create(userId, Guid.NewGuid(), 200m),
                TopUpTransaction.Create(Guid.NewGuid(), Guid.NewGuid(), 300m), // different user
                TopUpTransaction.Create(userId, Guid.NewGuid(), 300m) with { CreatedAt = DateTime.Now.AddMonths(-1)} // previous month
            );
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(_dbContextOptions))
        {
            var repository = new TopUpTransactionRepository(context);

            // Act
            var total = await repository.GetMonthlyTotalForUser(userId);

            // Assert
            total.Should().Be(300m);
        }
    }
}