using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;

namespace Onyx.ContractService.Application.ContractComments.Commands.CreateContractComment
{
    public class CreateContractCommentCommandValidator : AbstractValidator<CreateContractCommentCommand>
    {       
        public CreateContractCommentCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Comment)
                .NotEmpty().WithMessage("Comment must be specified!")
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
