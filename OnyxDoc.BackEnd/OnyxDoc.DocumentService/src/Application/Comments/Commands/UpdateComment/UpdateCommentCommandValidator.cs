using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Organisation must be specified!");
            RuleFor(v => v.Comment)
                .NotEmpty().WithMessage("Comment must be specified!")
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}


