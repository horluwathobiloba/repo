using FluentValidation; 

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceTypes
{
    public class UpdateProductServiceTypesCommandValidator : AbstractValidator<UpdateProductServiceTypesCommand>
    {
        public UpdateProductServiceTypesCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


