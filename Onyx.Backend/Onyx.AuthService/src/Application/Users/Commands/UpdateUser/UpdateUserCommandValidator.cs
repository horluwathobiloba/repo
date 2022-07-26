using FluentValidation;

namespace Onyx.AuthService.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
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

            //RuleFor(v => v.Email)
            //.MaximumLength(200)
            //.NotEmpty();


           // RuleFor(v => v.PhoneNumber)
           //.MaximumLength(14)
           //.NotEmpty();
        }
    }
}
