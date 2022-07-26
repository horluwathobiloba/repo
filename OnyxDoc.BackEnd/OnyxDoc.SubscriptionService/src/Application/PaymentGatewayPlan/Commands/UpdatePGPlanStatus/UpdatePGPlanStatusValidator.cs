using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGPlans.Commands
{
    class UpdatePGPlanStatusValidator : AbstractValidator<UpdatePGPlanStatusCommand>
    {
        public UpdatePGPlanStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionId).GreaterThan(0).WithMessage("Subscription must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Payment gateway plan identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
