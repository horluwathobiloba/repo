using FluentValidation;
using Onyx.AuthService.Application.Users.Commands.CreateUser;

namespace Onyx.AuthService.Application.Organizations.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
                .NotEmpty();

            RuleFor(v => v.RoleId)
                .NotEmpty();

            RuleFor(v => v.FirstName)
                 .MaximumLength(200)
                 .NotEmpty();

            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.Email)
            .MaximumLength(200)
            .NotEmpty();

           // RuleFor(v => v.PhoneNumber)
           //.MaximumLength(14)
           //.NotEmpty();
        }
    }
}
