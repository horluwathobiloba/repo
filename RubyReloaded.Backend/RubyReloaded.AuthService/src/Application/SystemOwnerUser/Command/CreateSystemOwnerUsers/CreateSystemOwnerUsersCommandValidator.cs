using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.SystemOwnerUser.Command.CreateSystemOwnerUsers
{
    public class CreateSystemOwnerUsersCommandValidator:AbstractValidator<CreateSystemOwnerUsersCommand>
    {
        public CreateSystemOwnerUsersCommandValidator()
        {
            RuleFor(v => v.SystemOwnerUserRequests)
            .NotEmpty();
        }
    }
}
