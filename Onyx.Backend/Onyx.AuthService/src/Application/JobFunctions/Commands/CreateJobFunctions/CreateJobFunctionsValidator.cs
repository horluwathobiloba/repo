using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.JobFunctions.Commands.CreateJobFunctions
{
    public class CreateJobFunctionsValidator:AbstractValidator<CreateJobFunctionsCommand>
    {
        public CreateJobFunctionsValidator()
        {
            RuleFor(v => v.OrganisationId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.JobFunctions).NotNull().WithMessage("Jobfunction requests cannot be null");
        }
    }
}
