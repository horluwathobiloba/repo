using FluentValidation; 

namespace Onyx.ContractService.Application.PaymentPlans.Commands.CreatePaymentPlan
{
    public class CreatePaymentPlanCommandValidator: AbstractValidator<CreatePaymentPlanCommand>
    {
        public CreatePaymentPlanCommandValidator()
        { 
             RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Payment plan name must be specified!")
                .MaximumLength(200).WithMessage("Payment plan name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
