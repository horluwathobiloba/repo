using FluentValidation;

namespace Onyx.ContractService.Application.Contracts.Queries.GetRenewedContract
{
    public class GetRenewedContractCommandValidator : AbstractValidator<GetRenewedContractCommand>
    {
        public GetRenewedContractCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!"); 
        }
    }
}
