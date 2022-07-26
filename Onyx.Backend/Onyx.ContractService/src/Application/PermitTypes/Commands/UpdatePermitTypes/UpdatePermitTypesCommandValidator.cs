using FluentValidation;
using Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.PermitTypes.Commands.UpdatePermitTypes
{
    public class UpdatePermitTypesCommandValidator : AbstractValidator<UpdatePermitTypesCommand>
    {
        public UpdatePermitTypesCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


