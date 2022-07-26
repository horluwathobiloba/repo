using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyEmail
{
    public class VerifiyEmailCommandValidator:AbstractValidator<VerifyEmailCommand>
    {
        public VerifiyEmailCommandValidator()
        {
            RuleFor(v => v.Email)
             .NotEmpty()
             .NotNull()
             .WithMessage("Invalid Email");

            RuleFor(v => v.OTP)
             .NotEmpty()
             .NotNull()
             .WithMessage("Otp value cannot be empty");
        }
    }
}
