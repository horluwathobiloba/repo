using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.CreateCooperativeUserMapping
{
    public class CreateCooperativeUserMappingValidator:AbstractValidator<CreateCooperativeUserMappingCommand>
    {
        public CreateCooperativeUserMappingValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty();

            RuleFor(v => v.CooperativeId)
                .NotEmpty();
            RuleFor(v => v.UserId)
           .NotEmpty();
        }
    }
}
