using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Comments.Commands.UpdateCommentStatus
{
    class UpdateCommentStatusValidator : AbstractValidator<UpdateCommentStatusCommand>
    {
        public UpdateCommentStatusValidator()
        {
             RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
