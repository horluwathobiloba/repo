using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Branding.Commands.UpdateBranding
{
    public class UpdateBrandingCommandValidator : AbstractValidator<UpdateBrandingCommand>
    {
        public UpdateBrandingCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid branding Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
