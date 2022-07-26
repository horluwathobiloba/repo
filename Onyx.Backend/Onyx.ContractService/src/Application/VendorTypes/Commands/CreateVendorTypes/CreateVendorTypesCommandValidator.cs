using FluentValidation; 

namespace Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorTypes
{
    public class CreateVendorTypesCommandValidator: AbstractValidator<CreateVendorTypesCommand>
    {
        public CreateVendorTypesCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            //RuleFor(v => v.Name)
            //    .NotEmpty().WithMessage("Vendor type name must be specified!")
            //    .MaximumLength(200).WithMessage("Vendor type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
