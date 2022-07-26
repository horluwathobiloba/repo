using FluentValidation;

namespace Onyx.AuthService.Application.Users.Commands.SendContractRequestEmail
{
    public class SendContractRequestEmailCommandValidator : AbstractValidator<SendContractRequestEmailCommand>
    {
        public SendContractRequestEmailCommandValidator()
        {
         
            RuleFor(v => v.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Invalid Email");

        }
    }
}
