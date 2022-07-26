using FluentValidation;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.ChangeProductCategoryConfigurationStatus
{
    public class ChangeProductCategoryConfigurationStatusValidator : AbstractValidator<ChangeProductCategoryConfigurationStatusCommand>
    {
        public ChangeProductCategoryConfigurationStatusValidator()
        {
            RuleFor(v => v.Id)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
