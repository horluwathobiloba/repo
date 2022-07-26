using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;

namespace Onyx.ContractService.Application.PermitTypes.Commands.CreatePermitType
{
    public class CreatePermitTypeCommandValidator : AbstractValidator<CreatePermitTypeCommand>
    {       
        public CreatePermitTypeCommandValidator()
        {

            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Permit type name must be specified!")
                .MaximumLength(200).WithMessage("Permit type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }


    }
}
