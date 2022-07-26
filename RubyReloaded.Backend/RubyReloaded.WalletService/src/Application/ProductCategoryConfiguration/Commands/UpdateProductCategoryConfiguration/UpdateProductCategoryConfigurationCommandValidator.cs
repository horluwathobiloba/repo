using FluentValidation;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.UpdateProductCategoryConfigurations
{
    public class UpdateProductCategoryConfigurationValidator : AbstractValidator<UpdateProductCategoryConfigurationCommand>
    {
        public UpdateProductCategoryConfigurationValidator()
        {
            RuleFor(v => v.Id)
               .NotEmpty();

            RuleFor(v => v.Name)
               .NotEmpty().NotNull();

            RuleFor(v => v.ProductCategory)
               .NotEmpty().NotNull();

            RuleFor(v => v.ServiceConfigurationId)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
