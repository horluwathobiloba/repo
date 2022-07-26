using FluentValidation;
using ReventInject;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidator()
        {
            RuleFor(v => v.Description).MaximumLength(100).WithMessage("Payment description must not exceed 100 characters").NotEmpty().WithMessage("Payment description must not be empty!"); 
            RuleFor(v => v.FeeRate.ToInt()).GreaterThanOrEqualTo(0).WithMessage("FeeRate must be specified!");              
        }
    }
}


