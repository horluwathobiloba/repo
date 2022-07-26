using FluentValidation; 

namespace Onyx.ContractService.Application.LicenseTypes.Commands.CreateLicenseTypes
{
    public class CreateLicenseTypesCommandValidator: AbstractValidator<CreateLicenseTypesCommand>
    {
        public CreateLicenseTypesCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
