using FluentAssertions;
using NSubstitute;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;
using Wigo.Infrastructure.Interfaces;
using Wigo.Service.Abstractions;
using Wigo.Service.Commands;
using Wigo.Service.Handlers;

namespace Wigo.Tests.UnitTests.Handlers;

public class AddTopUpTransactionHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly ITopUpTransactionRepository _topUpTransactionRepository;
    private readonly IExternalBalanceService _balanceService;
    private readonly AddTopUpTransactionHandler _handler;

    public AddTopUpTransactionHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _beneficiaryRepository = Substitute.For<IBeneficiaryRepository>();
        _topUpTransactionRepository = Substitute.For<ITopUpTransactionRepository>();
        _balanceService = Substitute.For<IExternalBalanceService>();

        _handler = new AddTopUpTransactionHandler(
            _userRepository,
            _beneficiaryRepository,
            _topUpTransactionRepository,
            _balanceService);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var command = new AddTopUpTransactionCommand(
            UserId: Guid.NewGuid(),
            BeneficiaryId: Guid.NewGuid(),
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);
        _userRepository.GetUserByIAsync(command.UserId).Returns((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("User not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBeneficiaryNotFoundOrNotBelongsToUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);

        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns((Beneficiary)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Beneficiary not found or does not belong to the user.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenInsufficientBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);

        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        var beneficiary = Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId };
        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns(beneficiary);
        _balanceService.GetBalanceAsync(command.UserAccountBalanceNumber).Returns(99);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Insufficient balance.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceedMonthlyTopUpLimitPerBeneficiary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 900);

        decimal maxPerBeneficiary = 1000; // or 500 if user is verified
        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        var beneficiary = Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId };
        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns(beneficiary);
        _balanceService.GetBalanceAsync(command.UserAccountBalanceNumber).Returns(1000);
        _topUpTransactionRepository.GetMonthlyTotalForBeneficiary(command.UserId, command.BeneficiaryId).Returns(maxPerBeneficiary);
        _topUpTransactionRepository.GetMonthlyTotalForUser(command.UserId).Returns(2000);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be($"Exceeded monthly top-up limit per beneficiary of AED {maxPerBeneficiary}.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceedMonthlyTopUpLimitForAllBeneficiaries()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);

        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        var beneficiary = Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId };
        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns(beneficiary);
        _balanceService.GetBalanceAsync(command.UserAccountBalanceNumber).Returns(1500);
        _topUpTransactionRepository.GetMonthlyTotalForBeneficiary(command.UserId, command.BeneficiaryId).Returns(200);
        _topUpTransactionRepository.GetMonthlyTotalForUser(command.UserId).Returns(3000);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Exceeded monthly top-up limit for all beneficiaries of AED 3000.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFailedToDebitBalance()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);

        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        var beneficiary = Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId };
        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns(beneficiary);
        _balanceService.GetBalanceAsync(command.UserAccountBalanceNumber).Returns(1000);
        _topUpTransactionRepository.GetMonthlyTotalForBeneficiary(command.UserId, command.BeneficiaryId).Returns(0);
        _topUpTransactionRepository.GetMonthlyTotalForUser(command.UserId).Returns(0);
        _balanceService.DebitBalanceAsync(command.UserAccountBalanceNumber, command.Amount).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Failed to debit balance.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTransactionIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var command = new AddTopUpTransactionCommand(
            UserId: userId,
            BeneficiaryId: beneficiaryId,
            UserAccountBalanceNumber: "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562",
            Amount: 100);

        var user = User.Create(Faker.Name.FullName()) with { Id = userId };
        var beneficiary = Beneficiary.Create(userId, Faker.Name.FullName(), Faker.Phone.Number()) with { Id = beneficiaryId };
        var topUpTransactionId = Guid.NewGuid();

        _userRepository.GetUserByIAsync(command.UserId).Returns(user);
        _beneficiaryRepository.GetBeneficiaryByIdAsync(command.BeneficiaryId).Returns(beneficiary);
        _balanceService.GetBalanceAsync(command.UserAccountBalanceNumber).Returns(1000);
        _topUpTransactionRepository.GetMonthlyTotalForBeneficiary(command.UserId, command.BeneficiaryId).Returns(0);
        _topUpTransactionRepository.GetMonthlyTotalForUser(command.UserId).Returns(0);
        _balanceService.DebitBalanceAsync(command.UserAccountBalanceNumber, command.Amount).ReturnsForAnyArgs(true);
        _topUpTransactionRepository.AddTopUpTransactionAsync(Arg.Any<TopUpTransaction>()).Returns(topUpTransactionId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ServiceResult<Guid>>();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(topUpTransactionId);
    }
}