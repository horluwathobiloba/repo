using FluentValidation;
using RubyReloaded.AuthService.Application.Clients.Commands.GenerateAPIKey;

namespace RubyReloaded.AuthService.Application.Clients.Commands.GenerateAPIKey
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
