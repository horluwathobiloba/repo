using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommandValidator:AbstractValidator<UpdatePhoneNumberCommand>
    {
        public UpdatePhoneNumberCommandValidator()
        {
            RuleFor(v => v.Email)
               .MaximumLength(200)
               .NotEmpty();
            RuleFor(v => v.PhoneNumber)
                 .MaximumLength(200)
                 .NotEmpty();
        }
    }
}
