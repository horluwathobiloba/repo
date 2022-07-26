using FluentValidation;
using OnyxDoc.DocumentService.Application.Documents.Commands.SendToDocumentSignatories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Documents.Commands.SendDocumentExpirationNotification
{
    public class SendDocumentExpirationNotificationValidator : AbstractValidator<SendToDocumentSignatoriesCommand>
    {
        public SendDocumentExpirationNotificationValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber Id must be specified!");
            RuleFor(v => v.DocumentId).NotEqual(0).WithMessage("Document Id must be specified!");
        }
    }
}
