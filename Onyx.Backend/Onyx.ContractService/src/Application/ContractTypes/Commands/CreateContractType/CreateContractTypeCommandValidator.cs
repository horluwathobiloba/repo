using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.ContractService.Application.ContractTypes.Commands.CreateContractType
{
    public class CreateContractTypeCommandValidator:AbstractValidator<CreateContractTypeCommand>
    {
        public CreateContractTypeCommandValidator()
        {
             RuleFor(v => v.OrganisationId).GreaterThan(0).GreaterThan(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Contract type name must be specified!")
                .MaximumLength(200).WithMessage("Contract type name cannot exceed 200 characters length!");
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
            //    RuleFor(v => v.NumberOfWitnessSignatories ).GreaterThan(0).WithMessage("Number of witness signatories must be specified.");
            //});

            //RuleFor(v => v.TemplateFilePath).NotEmpty().WithMessage("Contract type template upload is required!");
        }
    }
}
