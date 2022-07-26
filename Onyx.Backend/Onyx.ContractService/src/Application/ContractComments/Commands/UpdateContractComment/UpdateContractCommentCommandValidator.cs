using FluentValidation;
using Onyx.ContractService.Application.ContractComments.Commands.UpdateContractComment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractComments.Commands.UpdateContractComment
{
    public class UpdateContractCommentCommandValidator : AbstractValidator<UpdateContractCommentCommand>
    {
        public UpdateContractCommentCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Comment)
                .NotEmpty().WithMessage("Comment must be specified!")
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


