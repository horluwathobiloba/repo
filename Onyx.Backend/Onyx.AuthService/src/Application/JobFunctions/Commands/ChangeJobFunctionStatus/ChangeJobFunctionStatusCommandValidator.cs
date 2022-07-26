using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.JobFunctions.Commands.ChangeJobFunctionStatus
{
    public class ChangeJobFunctionStatusCommandValidator: AbstractValidator<ChangeJobFunctionStatusCommand>
    {
        public ChangeJobFunctionStatusCommandValidator()
        {
            RuleFor(v => v.OrganizationId)
              .NotEmpty();
            RuleFor(v => v.UserId)
                .NotEmpty();
            RuleFor(v => v.JobFunctionId)
              .NotEmpty();
        }
    }
}
