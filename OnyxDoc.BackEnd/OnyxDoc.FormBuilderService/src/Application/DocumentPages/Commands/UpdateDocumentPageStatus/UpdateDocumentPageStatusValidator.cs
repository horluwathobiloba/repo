using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Commands
{
    class UpdateDocumentPageStatusValidator : AbstractValidator<UpdateDocumentPageStatusCommand>
    {
        public UpdateDocumentPageStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.DocumentId).GreaterThan(0).WithMessage("Document must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Document page identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User identifier must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
