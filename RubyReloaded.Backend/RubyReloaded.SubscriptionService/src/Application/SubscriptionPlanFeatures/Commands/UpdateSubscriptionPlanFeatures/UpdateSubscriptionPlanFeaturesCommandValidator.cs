using FluentValidation;
using RubyReloaded.SubscriptionService.Application.Common.Models;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class UpdateSubscriptionPlanFeaturesCommandValidator : AbstractValidator<UpdateSubscriptionPlanFeaturesCommand>
    {
        public UpdateSubscriptionPlanFeaturesCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Subscription plan must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
    public class UpdateSubscriptionFeatureRequestValidator : AbstractValidator<UpdateSubscriptionPlanFeatureRequest>
    {
        public UpdateSubscriptionFeatureRequestValidator()
        {
            RuleFor(v => v.FeatureId).NotEqual(0).WithMessage("Invalid feature specified!");
            RuleFor(v => v.FeatureName).NotEmpty().WithMessage("Feature name must be specified!");
        }
    }
}


