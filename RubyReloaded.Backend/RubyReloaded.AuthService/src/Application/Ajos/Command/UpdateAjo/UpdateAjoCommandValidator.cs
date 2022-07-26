using FluentValidation;
using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Ajos.Command.UpdateAjo
{
    public class UpdateAjoCommandValidator : AbstractValidator<UpdateAjoCommand>
    {
        public UpdateAjoCommandValidator()
        {
            RuleFor(v => v.AmountToDisbursePerUser)
             .NotEmpty();
            RuleFor(v => v.AmountPerUser)
             .NotEmpty();
            RuleFor(v => v.CollectionAmount)
             .NotEmpty();
            RuleFor(v => v.CollectionCycle)
             .NotEmpty();
            RuleFor(v => v.Id)
                .NotEmpty()
                .NotNull()
               .NotEqual(0);
            RuleFor(v => v.StartDate)
                .NotEmpty();
            RuleFor(v => v.EndDate)
               .NotEmpty();
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
