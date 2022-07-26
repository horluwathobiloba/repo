using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class UpdatePGPricesCommandValidator : AbstractValidator<UpdatePGPricesCommand>
    {
        public UpdatePGPricesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePGPriceRequestValidator : AbstractValidator<UpdatePGPriceRequest>
    {
        public UpdatePGPriceRequestValidator()
        {
            RuleFor(v => v.SubscriptionPlanPricingId).NotEmpty().WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.PaymentGatewayPriceId).NotEmpty().WithMessage("Price unique identifier must be specified!");
            RuleFor(v => v.PaymentGatewayPriceCode).NotEmpty().WithMessage("Price unique code must be specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
        }
    }
}



