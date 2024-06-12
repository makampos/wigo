using FluentValidation.TestHelper;
using Wigo.Service.Commands;
using Wigo.Service.Validators;

namespace Wigo.Tests.UnitTests;

public class AddTopUpTransactionCommandValidatorTests
{
    private readonly AddTopUpTransactionCommandValidator _validator;

    public AddTopUpTransactionCommandValidatorTests()
    {
        _validator = new AddTopUpTransactionCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var command = new AddTopUpTransactionCommand(Guid.Empty, Guid.NewGuid(), "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562", 10m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("UserId is required.");
    }

    [Fact]
    public void Should_Have_Error_When_BeneficiaryId_Is_Empty()
    {
        var command = new AddTopUpTransactionCommand(Guid.NewGuid(), Guid.Empty, "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562", 10m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.BeneficiaryId)
            .WithErrorMessage("BeneficiaryId is required.");
    }

    [Fact]
    public void Should_Have_Error_When_UserAccountBalanceNumber_Is_Empty()
    {
        var command = new AddTopUpTransactionCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, 10m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserAccountBalanceNumber)
            .WithErrorMessage("UserAccountBalanceNumber is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Invalid()
    {
        var command = new AddTopUpTransactionCommand(Guid.NewGuid(), Guid.NewGuid(), "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562", 15m);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("Amount must be one of the predefined top-up options.");
    }

    [Theory]
    [
     InlineData(5),
     InlineData(10),
     InlineData(20),
     InlineData(30),
     InlineData(50),
     InlineData(75),
     InlineData(100)
    ]
    public void Should_Not_Have_Error_When_Amount_Is_Valid(decimal validAmount)
    {
        var command = new AddTopUpTransactionCommand(Guid.NewGuid(), Guid.NewGuid(), "a3e64a30e2d44ef8901239f5d37483d9-rrsVDQ-BC903562", validAmount);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }
}