using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Ajos.Command
{
    public class CreateAjoCommandValidator:AbstractValidator<CreateAjoCommand>
    {
        public CreateAjoCommandValidator()
        {
            RuleFor(v => v.StartDate)
             .NotEmpty();
            RuleFor(v => v.EndDate)
             .NotEmpty();
            RuleFor(v => v.AmountToDisbursePerUser)
             .NotEmpty();
            RuleFor(v => v.AmountPerUser)
                .NotEmpty();
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
