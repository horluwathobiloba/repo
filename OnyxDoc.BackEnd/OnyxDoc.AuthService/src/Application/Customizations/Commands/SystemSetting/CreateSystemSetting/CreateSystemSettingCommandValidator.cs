using FluentValidation;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.SystemSetting.Commands.CreateSystemSetting
{
    public class CreateSystemSettingCommandValidator:AbstractValidator<CreateSystemSettingCommand>
    {
        public CreateSystemSettingCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!"); 
        }

    }
}
