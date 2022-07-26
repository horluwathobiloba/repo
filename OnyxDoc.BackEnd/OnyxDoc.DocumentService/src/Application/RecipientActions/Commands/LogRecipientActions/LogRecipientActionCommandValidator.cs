using FluentValidation;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.RecipientActions.Commands.LogRecipientAction
{
    //public class LogRecipientActionCommandValidator : AbstractValidator<LogRecipientActionCommand>
    //{
    //    public LogRecipientActionCommandValidator()
    //    {
    //        RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
    //        RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid  must be specified!");
    //        RuleFor(v => v.RecipientId).GreaterThan(0).WithMessage("A valid recipient must be specified!");
    //        RuleFor(v => v.RecipientAction).IsInEnum().WithMessage("A valid recipient action must be specified!");
    //        RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");

    //    }
    //}
}
