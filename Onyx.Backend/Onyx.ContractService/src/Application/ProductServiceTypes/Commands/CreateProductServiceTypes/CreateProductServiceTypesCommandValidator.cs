using FluentValidation;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.CreateProductServiceTypes
{
    public class CreateProductServiceTypesCommandValidator : AbstractValidator<CreateProductServiceTypesCommand>
    {
        public CreateProductServiceTypesCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");              
        }
    }
}
