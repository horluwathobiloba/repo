using FluentValidation;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Commands.ChangeBankConfigurationStatus
{
    public class ChangeBankConfigurationStatusCommandValidator : AbstractValidator<ChangeBankConfigurationStatusCommand>
    {
        public ChangeBankConfigurationStatusCommandValidator()
        {
            RuleFor(v => v.BankConfigurationId)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
