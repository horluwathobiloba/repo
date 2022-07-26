using FluentValidation;
using Onyx.ContractService.Application.ContractTypes.Commands.ChangeContractTypeStatus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractTypeStatus
{
    public class UpdateContractTypeStatusCommandValidator : AbstractValidator<UpdateContractTypeStatusCommand>
    {
        public UpdateContractTypeStatusCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Contract type id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
