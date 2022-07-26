using FluentValidation;
using Onyx.ContractService.Application.PaymentPlans.Commands.UpdatePaymentPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.PaymentPlans.Commands.UpdatePaymentPlan
{
    public class UpdatePaymentPlanCommandValidator : AbstractValidator<UpdatePaymentPlanCommand>
    {
        public UpdatePaymentPlanCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Payment plan id must be specified!"); 
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Payment plan name must be specified!")
                .MaximumLength(200).WithMessage("Payment plan name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


