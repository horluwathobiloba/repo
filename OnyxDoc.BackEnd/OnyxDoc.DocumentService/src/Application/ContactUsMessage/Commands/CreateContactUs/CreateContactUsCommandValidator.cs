using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.ContactUsMessage.Commands.CreateContactUs
{
    public class CreateContactUsCommandValidator : AbstractValidator<CreateContactUsCommand>
    {
        public CreateContactUsCommandValidator()
        {
            RuleFor(v => v.Name).NotNull().WithMessage("Person's name must be specified");
            RuleFor(v => v.Email).NotNull().WithMessage("Person's email must be specified");
            RuleFor(v => v.Message).NotNull().WithMessage("Person's feedback must be specified");
        }
    }
}
