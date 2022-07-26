using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Cooperatives.Commands.CreateCooperative
{
    public class CreateCooperativeCommandValidator: AbstractValidator<CreateCooperativeCommand>
    {
        public CreateCooperativeCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.CooperativeType)
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
