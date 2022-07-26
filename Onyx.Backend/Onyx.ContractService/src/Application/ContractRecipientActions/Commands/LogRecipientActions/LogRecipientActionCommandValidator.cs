using FluentValidation;
using Onyx.ContractService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractRecipientActions.Commands.LogRecipientAction
{
    public class ApproveActionCommandValidator : AbstractValidator<LogRecipientActionCommand>
    {
        public ApproveActionCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("A valid Contract must be specified!");
            RuleFor(v => v.ContractRecipientId).GreaterThan(0).WithMessage("A valid recipient must be specified!");
            RuleFor(v => v.RecipientAction).IsInEnum().WithMessage("A valid recipient action must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");

        }
    }
}
