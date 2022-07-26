using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class CreatePGProductsCommandValidator : AbstractValidator<CreatePGProductsCommand>
    {
        public CreatePGProductsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!"); 
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!");         
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePGProductRequestValidator : AbstractValidator<CreatePGProductRequest>
    {
        public CreatePGProductRequestValidator()
        {
            RuleFor(v => v.PaymentGatewayProductCode).NotEmpty().WithMessage("Invalid product specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!"); 
        }
    }
}
