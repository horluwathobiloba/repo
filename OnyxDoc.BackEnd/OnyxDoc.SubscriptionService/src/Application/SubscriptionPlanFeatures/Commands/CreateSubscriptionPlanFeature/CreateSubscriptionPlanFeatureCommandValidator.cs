using FluentValidation; 

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class CreateSubscriptionPlanFeatureCommandValidator : AbstractValidator<CreateSubscriptionPlanFeatureCommand>
    {       
        public CreateSubscriptionPlanFeatureCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.SubscriptionPlanId).GreaterThan(0).WithMessage("Invalid subscription plan specified!"); 
            RuleFor(v => v.FeatureId).GreaterThan(0).WithMessage("Invalid feature specified!");
            RuleFor(v => v.FeatureName).NotEmpty().WithMessage("Feature name must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
