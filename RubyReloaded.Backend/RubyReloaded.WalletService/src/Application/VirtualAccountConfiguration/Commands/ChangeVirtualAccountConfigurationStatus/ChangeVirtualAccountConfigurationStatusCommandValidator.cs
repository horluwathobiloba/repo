using FluentValidation;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigs.Commands.ChangeVirtualAccountConfigStatus
{
    public class ChangeVirtualAccountConfigurationStatusValidator : AbstractValidator<ChangeVirtualAccountConfigurationStatusCommand>
    {
        public ChangeVirtualAccountConfigurationStatusValidator()
        {
            RuleFor(v => v.VirtualAccountConfigId)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
