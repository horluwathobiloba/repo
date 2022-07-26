using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {       
        public CreateCommentCommandValidator()
        {
            RuleFor(v => v.SubscriberId).NotEqual(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Comment)
                .NotEmpty().WithMessage("Comment must be specified!")
                .MaximumLength(200).WithMessage("Comment cannot exceed 200 characters length!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
