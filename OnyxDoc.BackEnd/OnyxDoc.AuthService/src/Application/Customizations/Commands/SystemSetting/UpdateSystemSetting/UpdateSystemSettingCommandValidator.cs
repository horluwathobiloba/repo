using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.SystemSetting.Commands.UpdateSystemSetting
{
    public class UpdateSystemSettingCommandValidator : AbstractValidator<UpdateSystemSettingCommand>
    {
        public UpdateSystemSettingCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid system setting Id must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!");
        }
    }
}
