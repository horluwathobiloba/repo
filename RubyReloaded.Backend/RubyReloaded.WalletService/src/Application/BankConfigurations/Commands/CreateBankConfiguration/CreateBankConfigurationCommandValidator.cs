using FluentValidation;

namespace RubyReloaded.WalletService.Application.BankConfigurations.Commands.CreateBankConfiguration
{
    public class CreateBankConfigurationCommandValidator : AbstractValidator<CreateBankConfigurationCommand>
    {
        public CreateBankConfigurationCommandValidator()
        {
            RuleFor(v => v.BankDtos)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
