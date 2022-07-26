using FluentValidation;
using Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.LicenseTypes.Commands.UpdateLicenseTypes
{
    public class UpdateLicenseTypeCommandValidator : AbstractValidator<UpdateLicenseTypeCommand>
    {
        public UpdateLicenseTypeCommandValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!"); 
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


