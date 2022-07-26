using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ProductServiceTypes.Commands.UpdateProductServiceTypeStatus
{
    public class UpdateProductServiceTypeStatusCommandValidator : AbstractValidator<UpdateProductServiceTypeStatusCommand>
    {
        public UpdateProductServiceTypeStatusCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Product service type id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
