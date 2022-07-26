using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGProducts.Commands
{
    public class UpdatePGProductsCommandValidator : AbstractValidator<UpdatePGProductsCommand>
    {
        public UpdatePGProductsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).NotEmpty().WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePGProductRequestValidator : AbstractValidator<UpdatePGProductRequest>
    {
        public UpdatePGProductRequestValidator()
        {
            RuleFor(v => v.PaymentGatewayProductCode).NotEmpty().WithMessage("Invalid product specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
        }
    }
}



