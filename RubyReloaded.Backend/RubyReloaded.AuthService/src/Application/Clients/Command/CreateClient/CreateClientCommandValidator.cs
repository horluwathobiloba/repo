using FluentValidation;
using RubyReloaded.AuthService.Application.Clients.Commands.CreateClient;

namespace RubyReloaded.AuthService.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();

        }
    }
}
