using FluentValidation; 
namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlans.Commands
{
    public class UpdateSubscriptionPlanCommandValidator : AbstractValidator<UpdateSubscriptionPlanCommand>
    {
        public UpdateSubscriptionPlanCommandValidator()
        {
            RuleFor(v => v.Id).NotEqual(0).WithMessage("Payment channel id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name must be specified!");
            RuleFor(v => v.SubscriptionType).IsInEnum().WithMessage("Subscription type must be specified!");
            RuleFor(v => v.StorageSizeType).IsInEnum().WithMessage("Storage size type must be specified!");
            RuleFor(v => v.StorageSize).GreaterThan(0).WithMessage("A valid Storage size must be specified!");
            RuleFor(v => v.NumberOfTemplates).GreaterThan(0).WithMessage("Number of templates must be specified!");
            RuleFor(v => v.NumberOfUsers).GreaterThan(0).WithMessage("Number of users must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }   
}


