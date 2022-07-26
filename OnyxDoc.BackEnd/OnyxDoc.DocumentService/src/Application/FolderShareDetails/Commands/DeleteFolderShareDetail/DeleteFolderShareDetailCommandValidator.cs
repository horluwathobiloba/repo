using FluentValidation;

namespace OnyxDoc.DocumentService.Application.FolderShareDetails.Commands.DeleteFolderShareDetail
{
    public class DeleteFolderShareDetailCommandValidator : AbstractValidator<DeleteFolderShareDetailCommand>
    {
        public DeleteFolderShareDetailCommandValidator()
        {
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
