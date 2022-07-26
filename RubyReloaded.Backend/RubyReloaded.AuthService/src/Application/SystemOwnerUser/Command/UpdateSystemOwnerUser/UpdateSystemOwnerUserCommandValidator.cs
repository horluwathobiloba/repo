using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Command.UpdateSuperAdmin
{
    public class UpdateSystemOwnerUserCommandValidator:AbstractValidator<UpdateSystemOwnerUserCommand>
    {
        public UpdateSystemOwnerUserCommandValidator()
        {
            RuleFor(v => v.FirstName)
                 .MaximumLength(200)
                 .NotEmpty();
            RuleFor(v => v.Email)
                .MaximumLength(200)
                 .NotEmpty();

            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.Email)
            .MaximumLength(200)
            .NotEmpty();

            RuleFor(v => v.PhoneNumber)
            .MinimumLength(7)
           .MaximumLength(15)
           .NotEmpty();
        }
    }
}
