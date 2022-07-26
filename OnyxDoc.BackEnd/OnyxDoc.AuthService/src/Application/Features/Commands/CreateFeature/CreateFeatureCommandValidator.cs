using FluentValidation;
using OnyxDoc.AuthService.Application.Features.Commands.CreateFeature;

namespace OnyxDoc.AuthService.Application.Features.Commands.CreateFeature
{
    public class CreateFeatureCommandValidator : AbstractValidator<CreateFeatureCommand>
    {
        public CreateFeatureCommandValidator()
        {
            RuleFor(v => v.SubscriberId)
              .NotEmpty();

            //RuleFor(v => v.UserId)
            //    .NotEmpty();

            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
