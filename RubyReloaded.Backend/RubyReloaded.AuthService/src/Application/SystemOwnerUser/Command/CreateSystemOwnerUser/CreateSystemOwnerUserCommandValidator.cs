using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.CreateSuperAdmin
{
    public class CreateSystemOwnerUserCommandValidator:AbstractValidator<CreateSystemOwnerUserCommand>
    {
        public CreateSystemOwnerUserCommandValidator()
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
