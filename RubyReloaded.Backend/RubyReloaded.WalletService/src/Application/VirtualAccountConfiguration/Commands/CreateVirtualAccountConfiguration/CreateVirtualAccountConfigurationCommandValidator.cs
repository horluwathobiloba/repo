using FluentValidation;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.CreateVirtualAccountConfig
{
    public class CreateVirtualAccountConfigurationValidator : AbstractValidator<CreateVirtualAccountConfigurationCommand>
    {
        public CreateVirtualAccountConfigurationValidator()
        {
            RuleFor(v => v.BankId)
               .NotEmpty().NotNull();

            RuleFor(v => v.SettlementAccount)
               .NotEmpty().NotNull();

            RuleFor(v => v.Currency)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
