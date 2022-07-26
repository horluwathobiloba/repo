using FluentValidation;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.CreateAjoRole
{
    public class CreateAjoRoleCommandValidator:AbstractValidator<CreateAjoRoleCommand>
    {
        public CreateAjoRoleCommandValidator()
        {
            RuleFor(v => v.AjoId)
               .NotEmpty();

            RuleFor(v => v.AccessLevel)
                .NotEmpty();
            RuleFor(v => v.Name)
             .NotEmpty()
             .NotNull();
         
        }
    }
}
