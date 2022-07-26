using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.JobFunctions.Commands
{
    public class UpdateJobFunctionValidator : AbstractValidator<UpdateJobFunctionCommand>
    {
        public UpdateJobFunctionValidator()
        {
            RuleFor(v => v.Name)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.UserId)
                .NotNull()
                .NotEmpty();
        }
    }
}
