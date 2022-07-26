using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
    {
        public UpdateContractCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.ContractId).GreaterThan(0).WithMessage("Please select a valid contract for edit!");  
            When(v => v.ContractTypeId > 0, () =>
            {
                RuleFor(v => v.ContractTypeId).GreaterThan(0).WithMessage("A valid contract type must be specified.");
            });
            RuleFor(v => v.VendorId).GreaterThan(0).WithMessage("A valid Vendor must be specified!");
            RuleFor(v => v.ProductServiceTypeId).GreaterThan(0).WithMessage("A valid Product Service Type must be specified!");
            //should this be compulsory - Do all contract require payment?
            RuleFor(v => v.PaymentPlanId).GreaterThan(0).WithMessage("A valid payment plan must be specified!");
            RuleFor(v => v.VendorTypeId).GreaterThan(0).WithMessage("A valid Vendor Type is must be specified!");
            RuleFor(v => v.Email).NotEmpty().WithMessage("Email must be specified!");
            RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Phone number must be specified!");
            RuleFor(v => v.SupplierClass).NotEmpty().WithMessage("Supplier class must be specified!");
            RuleFor(v => v.State).NotEmpty().WithMessage("State must be specified!");
            RuleFor(v => v.Country).NotEmpty().WithMessage("Country must be specified!");
            RuleFor(v => v.SupplierCode).NotEmpty().WithMessage("Supplier Code must be specified!");
            //RuleFor(v => v.ShortName).NotEmpty().WithMessage("Short Name must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
