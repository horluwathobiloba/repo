using FluentValidation;
using OnyxDoc.AuthService.Application.Users.Commands.UpdateUser;

namespace OnyxDoc.AuthService.Application.Features.Commands.UpdateFeature
{
    public class UpdateFeatureCommandValidator : AbstractValidator<UpdateFeatureCommand>
    {
        public UpdateFeatureCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
               .NotEmpty();

            RuleFor(v => v.UserId)
                .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
