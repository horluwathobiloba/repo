using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models; 

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class CreatePGCustomersCommandValidator : AbstractValidator<CreatePGCustomersCommand>
    {
        public CreatePGCustomersCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber identifier must be specified!");         
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }

    public class CreatePGCustomerRequestValidator : AbstractValidator<CreatePGCustomerRequest>
    {
        public CreatePGCustomerRequestValidator()
        {
            RuleFor(v => v.PaymentGatewayCustomerId).NotEmpty().WithMessage("Invalid payment gateway customer unique identifier specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!"); 
        }
    }
}
