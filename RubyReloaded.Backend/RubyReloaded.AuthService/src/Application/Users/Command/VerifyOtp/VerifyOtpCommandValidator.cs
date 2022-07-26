using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.VerifyOtp
{
    public class VerifyOtpCommandValidator:AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(v => v.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid Email");

            RuleFor(v => v.OTP)
             .NotEmpty()
             .NotNull()
             .WithMessage("Token value cannot be empty");
        }
    }
}
