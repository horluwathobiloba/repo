using FluentValidation;

namespace Onyx.ContractService.Application.Contracts.Commands.CreateContract
{
    public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
    {
        public CreateContractCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name).NotEmpty().NotNull().WithMessage("A valid Contract Name must be specified!");
            //RuleFor(v => v.VendorId).GreaterThan(0).WithMessage("A valid Vendor must be specified!");
            ////RuleFor(v => v.ProductServiceTypeId).GreaterThan(0).WithMessage("A valid Product Service Type must be specified!");

            //////should this be compulsory - Do all contract require payment?
            ////RuleFor(v => v.PaymentPlanId).GreaterThan(0).WithMessage("A valid Payment Plan must be specified!");
            ////RuleFor(v => v.VendorTypeId).GreaterThan(0).WithMessage("A valid Vendor Type is must be specified!");
            ////RuleFor(v => v.Email).NotEmpty().WithMessage("Email must be specified!");
            ////RuleFor(v => v.PhoneNumber).NotEmpty().WithMessage("Phone number must be specified!");
            ////RuleFor(v => v.SupplierClass).NotEmpty().WithMessage("Supplier class must be specified!");
            ////RuleFor(v => v.State).NotEmpty().WithMessage("State must be specified!");
            ////RuleFor(v => v.Country).NotEmpty().WithMessage("Country must be specified!");
            ////RuleFor(v => v.SupplierCode).NotEmpty().WithMessage("Supplier code must be specified!");
            ////RuleFor(v => v.ShortName).NotEmpty().WithMessage("Short Name must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
