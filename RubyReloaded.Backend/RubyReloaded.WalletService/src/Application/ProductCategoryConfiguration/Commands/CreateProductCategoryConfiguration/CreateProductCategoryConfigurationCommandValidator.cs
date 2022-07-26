using FluentValidation;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Commands.CreateProductCategoryConfiguration
{
    public class CreateProductCategoryConfigurationCommandValidator : AbstractValidator<CreateProductCategoryConfigurationCommand>
    {
        public CreateProductCategoryConfigurationCommandValidator()
        {
            RuleFor(v => v.ProductCategory)
               .NotEmpty().NotNull();

            RuleFor(v => v.ServiceConfigurationId)
               .NotEmpty().NotNull();

            RuleFor(v => v.UserId)
                .NotEmpty();
        }
    }
}
