using FluentValidation;
using OnyxDoc.AuthService.Application.SignUp;

namespace OnyxDoc.AuthService.Application.Commands.SignUp
{
    public class CheckDomainNameCommandValidator : AbstractValidator<CheckUpDomainNameCommand>
    {
        public CheckDomainNameCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty()
                .WithMessage("Email field must not be empty ");
            RuleFor(v => v.Email)
                .EmailAddress()
                .WithMessage("Email address is not valid");
        }
    }
}
