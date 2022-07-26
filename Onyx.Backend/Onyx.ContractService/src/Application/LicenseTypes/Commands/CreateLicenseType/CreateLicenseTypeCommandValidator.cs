using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseType
{
    public class CreateLicenseTypeCommandValidator : AbstractValidator<CreateLicenseTypeCommand>
    {       
        public CreateLicenseTypeCommandValidator()
        {

            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("License type name must be specified!")
                .MaximumLength(200).WithMessage("License type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }


    }
}
