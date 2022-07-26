using FluentValidation;
using Onyx.AuthService.Application.Clients.Commands.CreateClient;

namespace Onyx.AuthService.Application.Clients.Commands.CreateClient
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
