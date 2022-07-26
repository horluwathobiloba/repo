using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.JobFunctions.Commands
{
    public class CreateJobFunctionValidator : AbstractValidator<CreateJobFunctionCommand>
    {
        public CreateJobFunctionValidator()
        {
            RuleFor(v => v.Name)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.OrganisationId)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}
