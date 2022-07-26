using FluentValidation; 

namespace Onyx.ContractService.Application.PaymentPlans.Commands.UpdatePaymentPlans
{
    public class UpdatePaymentPlansCommandValidator : AbstractValidator<UpdatePaymentPlansCommand>
    {
        public UpdatePaymentPlansCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            //RuleFor(v => v.Id).GreaterThan(0).WithMessage("Vendor type id must be specified!"); 
            //RuleFor(v => v.Name)
            //    .NotEmpty().WithMessage("Vendor type name must be specified!")
            //    .MaximumLength(200).WithMessage("Vendor type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


