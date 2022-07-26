using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands
{
    public class CreatePaymentChannelsCommandValidator: AbstractValidator<CreatePaymentChannelsCommand>
    {
        public CreatePaymentChannelsCommandValidator()
        { 
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePaymentChannelRequestValidator : AbstractValidator<CreatePaymentChannelRequest>
    {
        public CreatePaymentChannelRequestValidator()
        { 
            RuleFor(v => v.Name).NotEmpty().WithMessage("Payment channel name must be specified!");
            RuleFor(v => v.CurrencyCode).IsInEnum().WithMessage("Invalid Currency code specified!");
            RuleFor(v => v.TransactionRateType).IsInEnum().WithMessage("Invalid transaction rate type specified!");
            RuleFor(v => v.TransactionFee).NotEqual(0).WithMessage("Transaction fee must be specified!"); 
        }
    }
}
