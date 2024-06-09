using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Commands;
using Wigo.Service.Handlers;

namespace Wigo.Tests.UnitTests.Handlers;

public class AddBeneficiaryCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly AddBeneficiaryCommandHandler _handler;

    public AddBeneficiaryCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _beneficiaryRepository = Substitute.For<IBeneficiaryRepository>();
        _handler = new AddBeneficiaryCommandHandler(_userRepository, _beneficiaryRepository);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessResult_When_Beneficiary_Is_Created()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddBeneficiaryCommand(userId, Faker.Name.FullName(), Faker.Phone.Number());

        _userRepository.GetUserByIAsync(Arg.Any<Guid>()).Returns(User.Create(Faker.Name.FullName()) with { Id = userId });
        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(Arg.Any<Guid>()).Returns(new List<Beneficiary>());
        _beneficiaryRepository.AddBeneficiaryAsync(Arg.Any<Beneficiary>()).Returns(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_FailureResult_When_User_Is_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddBeneficiaryCommand(userId, Faker.Name.FullName(), Faker.Phone.Number());

        _userRepository.GetUserByIAsync(Arg.Any<Guid>()).Returns((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Failure.Should().BeTrue();
        result.ErrorMessage.Should().Be("User not found.");
    }

    [Fact]
    public async Task Handle_Should_Return_FailureResult_When_User_Has_Maximum_Beneficiaries()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddBeneficiaryCommand(userId, Faker.Name.FullName(), Faker.Phone.Number());

        _userRepository.GetUserByIAsync(Arg.Any<Guid>()).Returns(User.Create(Faker.Name.FullName()) with { Id = userId});
        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(Arg.Any<Guid>()).Returns(
            new List<Beneficiary>(SeedBeneficiaries(userId, 5)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Failure.Should().BeTrue();
        result.ErrorMessage.Should().Be("User cannot have more than 5 beneficiaries.");
    }

    [Fact]
    public async Task Handle_Should_Return_FailureResult_When_Beneficiary_Creation_Fails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new AddBeneficiaryCommand(userId, Faker.Name.FullName(), Faker.Phone.Number());

        _userRepository.GetUserByIAsync(Arg.Any<Guid>()).Returns(User.Create(Faker.Name.FullName()) with { Id = userId});
        _beneficiaryRepository.GetBeneficiariesByUserIdAsync(Arg.Any<Guid>()).Returns(new List<Beneficiary>());
        _beneficiaryRepository.AddBeneficiaryAsync(Arg.Any<Beneficiary>()).Throws(new Exception("Failed to create beneficiary"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Failure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Failed to create beneficiary: Failed to create beneficiary");
    }

    private static IEnumerable<Beneficiary> SeedBeneficiaries(Guid userId, int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number());
        }
    }
}