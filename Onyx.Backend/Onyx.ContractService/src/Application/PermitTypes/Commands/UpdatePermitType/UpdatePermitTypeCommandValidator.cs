using FluentValidation;
using Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypes
{
    public class UpdatePermitTypeCommandValidator : AbstractValidator<UpdatePermitTypeCommand>
    {
        public UpdatePermitTypeCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


