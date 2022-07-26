using FluentValidation;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class CreateStripeCardPaymentCommandValidator : AbstractValidator<CreateStripeCardPaymentCommand>
    {
        public CreateStripeCardPaymentCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEmpty().WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.Mode).NotEmpty().WithMessage("Payment mode must be specified!");
            RuleFor(v => v.CurrencyCode).NotEmpty().WithMessage("Currency code cannot be empty and must be specified!");
            RuleFor(v => v.Description).MaximumLength(100).WithMessage("Payment description must not exceed 200 characters").NotEmpty().WithMessage("Payment description must not be empty!");
            //RuleFor(v => v.TransactionFee.ToInt()).NotEqual(0).WithMessage("Transction fee must be specified!"); 
            // RuleFor(v => v.Amount).NotEqual(0).GreaterThan(0).WithMessage("Payment amount must be greater than zero!");
            //RuleFor(v => v.TransactionFee).NotEqual(0).GreaterThan(0).WithMessage("Transction fee must be greater than zero!");
        }

    }
}
