using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.CooperativeSignUp
{
    public class CooperativeSignUpCommandValidator : AbstractValidator<CooperativeSignUpCommand>
    {
        public CooperativeSignUpCommandValidator()
        {
            RuleFor(v => v.FirstName)
               .MaximumLength(200)
               .NotEmpty();
            
            RuleFor(v => v.CoopName)
               .MaximumLength(200)
               .NotEmpty();

            RuleFor(v => v.CoopEmail)
               .NotEmpty();
            RuleFor(v => v.UserEmail)
            .MaximumLength(200)
            .NotEmpty();

            RuleFor(v => v.CoopPhoneNumber)
           .MinimumLength(7)
           .MaximumLength(15)
           .NotEmpty();
            RuleFor(v => v.UserPhoneNumber)
           .MinimumLength(7)
           .MaximumLength(15)
           .NotEmpty();

        }
    }
}
