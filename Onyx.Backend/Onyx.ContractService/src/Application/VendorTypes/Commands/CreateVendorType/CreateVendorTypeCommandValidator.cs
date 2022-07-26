using FluentValidation;
using Onyx.ContractService.Application.Common.Interfaces;

namespace Onyx.ContractService.Application.VendorTypes.Commands.CreateVendorType
{
    public class CreateVendorTypeCommandValidator : AbstractValidator<CreateVendorTypeCommand>
    {       
        public CreateVendorTypeCommandValidator()
        {

            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Vendor type name must be specified!")
                .MaximumLength(200).WithMessage("Vendor type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }


    }
}
