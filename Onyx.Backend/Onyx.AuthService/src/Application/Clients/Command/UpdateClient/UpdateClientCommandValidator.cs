using FluentValidation;
using Onyx.AuthService.Application.Clients.Commands.UpdateClient;

namespace Onyx.AuthService.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();


        }
    }
}
