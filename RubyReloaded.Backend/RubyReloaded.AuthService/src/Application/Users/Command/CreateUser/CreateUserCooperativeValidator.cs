using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.User.Command.CreateUser
{
    public class CreateUserCooperativeValidator : AbstractValidator<CreateUserCooperativeCommand>
    {
        public CreateUserCooperativeValidator()
        {
            RuleFor(v => v.FirstName)
                 .MaximumLength(200)
                 .NotEmpty();
            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.Email)
             .MaximumLength(200)
             .NotEmpty();

        }
    }
}
