using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.CreateOTP
{
    public class CreateOTPCommandValidator:AbstractValidator<CreateOTPCommand>
    {
        public CreateOTPCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty();
            RuleFor(v => v.Reason)
                .NotEmpty();
        }
    }
}
