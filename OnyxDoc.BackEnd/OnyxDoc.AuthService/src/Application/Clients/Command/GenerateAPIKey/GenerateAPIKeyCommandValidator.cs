using FluentValidation;
using OnyxDoc.AuthService.Application.Clients.Commands.GenerateAPIKey;

namespace OnyxDoc.AuthService.Application.Clients.Commands.GenerateAPIKey
{
    public class GenerateAPIKeyCommandValidator : AbstractValidator<GenerateAPIKeyCommand>
    {
        public GenerateAPIKeyCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();

            //RuleFor(v => v.ApplicationType)
            //    .NotEmpty();

        }
    }
}
