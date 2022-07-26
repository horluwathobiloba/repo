using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Inboxes.Commands.CreateInboxes
{
    public class CreateInboxesValidator : AbstractValidator<CreateInboxesCommand>
    {
        public CreateInboxesValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Inboxes).NotEmpty().WithMessage("Inboxes cannot be null");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
