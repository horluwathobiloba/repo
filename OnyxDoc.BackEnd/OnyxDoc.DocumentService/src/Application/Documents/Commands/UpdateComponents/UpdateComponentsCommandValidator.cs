using FluentValidation;
using OnyxDoc.DocumentService.Application.Documents.Commands.UpdateComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.UpdateComponents
{
    public class UpdateComponentsCommandValidator : AbstractValidator<UpdateComponentsCommand>
    {
        public UpdateComponentsCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
           
        }
    }
}
