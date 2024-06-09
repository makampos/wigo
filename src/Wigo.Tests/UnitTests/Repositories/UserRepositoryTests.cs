using FluentAssertions;
using NSubstitute;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Interfaces;

namespace Wigo.Tests.UnitTests.Repositories;

public class UserRepositoryTests
{
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
    }

    [Fact]
    public async Task AddUserAsync_Should_Add_User()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(
            name: Faker.Name.FullName()) with { Id = userId};

        _userRepository.AddUserAsync(user).Returns(userId);

        // Act
        var retrievedUser = await _userRepository.AddUserAsync(user);

        // Assert
        retrievedUser.Should().NotBeEmpty();
        retrievedUser.Should().Be(userId);
    }

    [Fact]
    public async Task GetUserByIAsync_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(
            name: Faker.Name.FullName()) with { Id = userId };

        _userRepository.GetUserByIAsync(userId).Returns(user);

        // Act
        var retrievedUser = await _userRepository.GetUserByIAsync(userId);

        // Assert
        retrievedUser.Should().NotBeNull();
        retrievedUser.Name.Should().Be(user.Name);
        retrievedUser.Id.Should().Be(user.Id);
    }
}