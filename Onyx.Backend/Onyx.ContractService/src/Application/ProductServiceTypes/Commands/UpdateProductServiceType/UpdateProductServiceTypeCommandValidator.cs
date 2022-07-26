using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceType
{
    public class UpdateProductServiceTypeCommandValidator : AbstractValidator<UpdateProductServiceTypeCommand>
    {
        public UpdateProductServiceTypeCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Product service type id must be specified!");
            RuleFor(v => v.VendorTypeId).GreaterThan(0).WithMessage("Vendor type must be specified!");
            RuleFor(v => v.Name)
                    .NotEmpty().WithMessage("Contract type name must be specified!")
                    .MaximumLength(200).WithMessage("Contract type name cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");

        }
    }
}
