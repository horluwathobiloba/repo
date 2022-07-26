using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionCommandValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(v => v.UserId)
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
