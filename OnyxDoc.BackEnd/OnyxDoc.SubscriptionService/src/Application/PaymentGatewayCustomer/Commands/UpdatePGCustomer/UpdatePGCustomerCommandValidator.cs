using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class UpdatePGCustomerCommandValidator : AbstractValidator<UpdatePGCustomerCommand>
    {
        public UpdatePGCustomerCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!"); 
            RuleFor(v => v.PaymentGateway).IsInEnum().WithMessage("Invalid payment gateway specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


