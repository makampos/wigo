using FluentValidation;
using Wigo.Domain.Enums;
using Wigo.Service.Commands;

namespace Wigo.Service.Validators;

public class AddTopUpTransactionCommandValidator : AbstractValidator<AddTopUpTransactionCommand>
{
    public AddTopUpTransactionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.BeneficiaryId)
            .NotEmpty().WithMessage("BeneficiaryId is required.");

        RuleFor(x => x.UserAccountBalanceNumber)
            .NotEmpty().WithMessage("UserAccountBalanceNumber is required.");

        RuleFor(x => x.Amount)
            .Must(BeAValidTopUpOption).WithMessage("Amount must be one of the predefined top-up options.");
    }

    private bool BeAValidTopUpOption(decimal amount)
    {
        return Enum.GetValues(typeof(TopUpOptionsEnum))
            .Cast<TopUpOptionsEnum>()
            .Select(e => (decimal)(int)e)
            .Contains(amount);
    }
}