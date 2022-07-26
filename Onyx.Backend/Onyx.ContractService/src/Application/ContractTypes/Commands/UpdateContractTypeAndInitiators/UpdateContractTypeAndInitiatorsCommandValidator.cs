using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypes.Commands.UpdateContractTypeAndInitiators
{
    public class UpdateContractTypeAndInitiatorsCommandValidator : AbstractValidator<UpdateContractTypeAndInitiatorsCommand>
    {
        public UpdateContractTypeAndInitiatorsCommandValidator()
        {
            RuleFor(v => v.OrganisationId).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Id).GreaterThan(0);
            RuleFor(v => v.Name)
                .MaximumLength(200).WithMessage("Contract type name must not exceed 200 characters length!")
                .NotEmpty().WithMessage("Contract type name must be specified!");

            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");

            //When(v => v.EnableInternalSignatories == true, () =>
            //{
            //    RuleFor(v => v.NumberOfInternalSignatories).GreaterThan(0).WithMessage("Number of internal signatories must be specified.");
            //});

            //When(v => v.EnableExternalSignatories == true, () =>
            //{
            //    RuleFor(v => v.NumberOfExternalSignatories).GreaterThan(0).WithMessage("Number of external signatories must be specified.");
            //});

            //When(v => v.EnableThirdPartySignatories == true, () =>
            //{
            //    RuleFor(v => v.NumberOfThirdPartySignatories).GreaterThan(0).WithMessage("Number of third party signatories must be specified.");
            //});

            //When(v => v.EnableWitnessSignatories == true, () =>
            //{
            //    RuleFor(v => v.NumberOfWitnessSignatories).GreaterThan(0).WithMessage("Number of internal signatories must be specified.");
            //});

            RuleFor(v => v.TemplateFilePath).NotEmpty().WithMessage("Contract type template upload is required!");
        }
    }
}
