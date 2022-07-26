using FluentValidation;

namespace OnyxDoc.AuthService.Application.Features.Commands.ChangeFeature
{
    public class ChangeFeatureStatusCommandValidator : AbstractValidator<ChangeFeatureStatusCommand>
    {
        public ChangeFeatureStatusCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
              .NotEmpty();
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.FeatureId)
              .NotEmpty();
        }
    }
}
