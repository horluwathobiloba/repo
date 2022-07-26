using FluentValidation;
using Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTag;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTags.Commands.UpdateContractTags
{
    public class UpdateContractTagsCommandValidator : AbstractValidator<UpdateContractTagsCommand>
    {
        public UpdateContractTagsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


