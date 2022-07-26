using FluentValidation;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    class UpdateSubscriptionStatusesValidator : AbstractValidator<UpdateSubscriptionStatusesCommand>
    {
        public UpdateSubscriptionStatusesValidator()
        {
            RuleFor(v => v.Subscriptions).Null().WithMessage("Subscriber must be specified!");  
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!"); 
        }
    }

    class UpdateSubscriptionStatusesRequestValidator : AbstractValidator<UpdateSubscriptionStatusesRequest>
    {
        public UpdateSubscriptionStatusesRequestValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Subscription plan feature id must be specified!"); 
            RuleFor(v => v.SubscriptionStatus).IsInEnum().WithMessage("Subscription status must be specified!");
        }
    }
}
