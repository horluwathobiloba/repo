using FluentValidation;
using ReventInject;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands
{
    public class UpdatePaystackPaymentCommandValidator : AbstractValidator<UpdatePaystackPaymentCommand>
    {
        public UpdatePaystackPaymentCommandValidator()
        {
            RuleFor(v => v.ReferenceNo).NotEmpty().WithMessage("Reference number must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");              
        }
    }
}


