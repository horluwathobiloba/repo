using FluentValidation;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Branding.Commands.CreateBranding
{
    public class CreateBrandingCommandValidator:AbstractValidator<CreateBrandingCommand>
    {
        public CreateBrandingCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!"); 
        }

    }
}
