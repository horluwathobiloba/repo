using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class CreatePGCustomerCommandValidator : AbstractValidator<CreatePGCustomerCommand>
    {
        public CreatePGCustomerCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Invalid subscriber id specified!"); 
            RuleFor(v => v.PaymentGatewayCustomerId).NotEmpty().WithMessage("Invalid payment gateway customer unique identifier specified!");
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
