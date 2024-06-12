using FluentValidation.TestHelper;
using Wigo.Service.Commands;
using Wigo.Service.Validators;

namespace Wigo.Tests.UnitTests;

public class AddBeneficiaryCommandValidatorTests
    {
        private readonly AddBeneficiaryCommandValidator _validator;

        public AddBeneficiaryCommandValidatorTests()
        {
            _validator = new AddBeneficiaryCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var command = new AddBeneficiaryCommand(Guid.Empty, Faker.Name.Middle(), Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("User ID is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), Faker.Name.Middle(), Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_Nickname_Is_Null()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), null, Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Nickname)
                .WithErrorMessage("Nickname is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Nickname_Is_Empty()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), string.Empty, Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Nickname)
                .WithErrorMessage("Nickname is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Nickname_Exceeds_MaxLength()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), new string('a', 21), Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Nickname)
                .WithErrorMessage("Nickname must be 20 characters or fewer.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Nickname_Is_Valid()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), Faker.Name.Middle(), Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Nickname);
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Null()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), Faker.Name.Middle(), null);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Phone number is required.");
        }

        [Fact]
        public void Should_Have_Error_When_PhoneNumber_Is_Empty()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), Faker.Name.Middle(), string.Empty);
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Phone number is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
        {
            var command = new AddBeneficiaryCommand(Guid.NewGuid(), Faker.Name.Middle(), Faker.Phone.Number());
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }
    }