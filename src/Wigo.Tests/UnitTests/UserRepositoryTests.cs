using FluentAssertions;
using NSubstitute;
using Wigo.Domain.Entities;
using Wigo.Infrastructure.Interfaces;

namespace Wigo.Tests.UnitTests;

public class UserRepositoryTests
{
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
    }

    [Fact]
    public async Task CreateUserAsync_Should_Create_User()
    {
        // Arrange
        var user = User.Create(
            id: Guid.NewGuid(),
            name: Faker.Name.FullName(),
            beneficiaries: new List<Beneficiary>());

        _userRepository.GetUserByIAsync(user.Id).Returns(user);

        await _userRepository.CreateUserAsync(user);

        // Act
        var retrievedUser = await _userRepository.GetUserByIAsync(user.Id);

        // Assert
        retrievedUser.Should().NotBeNull();
        retrievedUser.Id.Should().Be(user.Id);
        retrievedUser.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task GetUserByIAsync_Should_Return_User_When_User_Exists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(
            id: userId,
            name: Faker.Name.FullName(),
            beneficiaries: new List<Beneficiary>());

        _userRepository.GetUserByIAsync(userId).Returns(user);

        // Act
        var result = await _userRepository.GetUserByIAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task GetUserByIdWithBeneficiariesIncludeAsync_Should_Return_User_With_Beneficiaries()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(
            id: userId,
            name: Faker.Name.FullName(),
            beneficiaries: new List<Beneficiary>
            {
                Beneficiary.Create(
                    id: Guid.NewGuid(),
                    userId: userId,
                    nickname: Faker.Name.Middle(),
                    phoneNumber: Faker.Phone.Number()),
                Beneficiary.Create(
                    id: Guid.NewGuid(),
                    userId: userId,
                    nickname: Faker.Name.Middle(),
                    phoneNumber: Faker.Phone.Number())
            });

        _userRepository.GetUserByIdWithBeneficiariesIncludeAsync(userId).Returns(user);

        // Act
        var result = await _userRepository.GetUserByIdWithBeneficiariesIncludeAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Beneficiaries.Should().HaveCount(2);
        result.Beneficiaries.Should().ContainSingle(b => b.Nickname == user.Beneficiaries.First().Nickname);
        result.Beneficiaries.Should().ContainSingle(b => b.Nickname == user.Beneficiaries.Last().Nickname);
    }
}