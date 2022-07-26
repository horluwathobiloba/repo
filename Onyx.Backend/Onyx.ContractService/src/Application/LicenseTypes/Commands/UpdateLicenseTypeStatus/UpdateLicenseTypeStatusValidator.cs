using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseTypeStatus
{
    class UpdateLicenseTypeStatusValidator : AbstractValidator<UpdateLicenseTypeStatusCommand>
    {
        public UpdateLicenseTypeStatusValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("License type id must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
