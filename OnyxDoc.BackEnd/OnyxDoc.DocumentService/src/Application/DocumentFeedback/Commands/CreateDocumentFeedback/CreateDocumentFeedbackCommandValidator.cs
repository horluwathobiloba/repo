using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.DocumentFeedback.Commands.CreateDocumentFeedback
{
    public class CreateDocumentFeedbackCommandValidator : AbstractValidator<CreateDocumentFeedbackCommand>
    {
        public CreateDocumentFeedbackCommandValidator()
        {
            RuleFor(v => v.DocumentId).NotEqual(0).WithMessage("Document Id must be specified!");
        }
    }
}
