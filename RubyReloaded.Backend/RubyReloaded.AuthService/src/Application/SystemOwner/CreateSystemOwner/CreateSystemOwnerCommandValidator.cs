using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwner.CreateSystemOwner
{
    public class CreateSystemOwnerCommandValidator : AbstractValidator<CreateSystemOwnerCommand>
    {
        public CreateSystemOwnerCommandValidator()
        {
            RuleFor(v => v.ContactEmail)
          .NotEmpty();

            RuleFor(v => v.ContactPhoneNumber)
             .NotEmpty();
            RuleFor(v => v.RCNumber)
             .NotEmpty();
            RuleFor(v => v.Name)
             .MaximumLength(200)
              .NotEmpty();
        }
    }
}
