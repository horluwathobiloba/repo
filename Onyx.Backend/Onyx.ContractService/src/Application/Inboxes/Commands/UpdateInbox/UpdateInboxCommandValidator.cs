using FluentValidation;
using Onyx.ContractService.Application.Inboxes.Commands.UpdateInbox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Inboxes.Commands.UpdateInboxes
{
    public class UpdateInboxCommandValidator : AbstractValidator<UpdateInboxCommand>
    {
        public UpdateInboxCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            //RuleFor(v => v.Id).NotEqual(0).WithMessage("Vendor type id must be specified!"); 
            //RuleFor(v => v.Name)
            //    .NotEmpty().WithMessage("Vendor type name must be specified!")
            //    .MaximumLength(200).WithMessage("Vendor type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


