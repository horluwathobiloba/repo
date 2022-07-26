using FluentValidation;
using OnyxDoc.AuthService.Application.Clients.Commands.UpdateClient;

namespace OnyxDoc.AuthService.Application.Clients.Commands.UpdateClient
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
