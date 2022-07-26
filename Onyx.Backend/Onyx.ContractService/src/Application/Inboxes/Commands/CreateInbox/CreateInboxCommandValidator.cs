using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;
using Onyx.ContractService.Application.Inboxs.Commands.CreateInbox;

namespace Onyx.ContractService.Application.Inboxes.Commands.CreateInbox
{
    public class CreateInboxCommandValidator : AbstractValidator<CreateInboxCommand>
    {       
        public CreateInboxCommandValidator()
        {

            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
         
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }


    }
}
