using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class UpdatePGCustomersCommandValidator : AbstractValidator<UpdatePGCustomersCommand>
    {
        public UpdatePGCustomersCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber identifier must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdatePGCustomerRequestValidator : AbstractValidator<UpdatePGCustomerRequest>
    {
        public UpdatePGCustomerRequestValidator()
        {
            RuleFor(v => v.PaymentGatewayCustomerCode).NotEmpty().WithMessage("Invalid customer unique identifier specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
        }
    }
}



