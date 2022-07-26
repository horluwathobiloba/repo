﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    class UpdateSubscriptionPlanFeatureStatusValidator : AbstractValidator<UpdateSubscriptionPlanFeatureStatusCommand>
    {
        public UpdateSubscriptionPlanFeatureStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Subscription plan feature id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
