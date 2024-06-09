using FluentValidation;
using Wigo.Service.Commands;

namespace Wigo.Service.Validators;

public class AddBeneficiaryCommandValidator : AbstractValidator<AddBeneficiaryCommand>
{
    public AddBeneficiaryCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Nickname)
            .NotEmpty().WithMessage("Nickname is required.")
            .MaximumLength(20).WithMessage("Nickname must be 20 characters or fewer.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");
    }
}