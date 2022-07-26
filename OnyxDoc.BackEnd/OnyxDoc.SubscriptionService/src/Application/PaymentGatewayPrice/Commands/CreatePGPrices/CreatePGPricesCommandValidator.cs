using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class CreatePGPricesCommandValidator : AbstractValidator<CreatePGPricesCommand>
    {
        public CreatePGPricesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");   
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePGPriceRequestValidator : AbstractValidator<CreatePGPriceRequest>
    {
        public CreatePGPriceRequestValidator()
        {
            RuleFor(v => v.SubscriptionPlanPricingId).GreaterThan(0).WithMessage("Invalid subscription plan pricing specified!");
            RuleFor(v => v.PaymentGatewayPriceCode).NotEmpty().WithMessage("Invalid price code identifier specified!");
            RuleFor(v => v.PaymentGatewayPriceId).NotEmpty().WithMessage("Invalid price unique identifier specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!"); 
        }
    }
}
