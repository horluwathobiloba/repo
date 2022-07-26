using FluentValidation; 

namespace Onyx.ContractService.Application.PermitTypes.Commands.CreatePermitTypes
{
    public class CreatePermitTypesCommandValidator: AbstractValidator<CreatePermitTypesCommand>
    {
        public CreatePermitTypesCommandValidator()
        { 
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
