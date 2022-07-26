using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolderSharedDetails
{
    public class CreateFolderSharedDetailCommandValidator : AbstractValidator<CreateFolderSharedDetailCommand>
    {
        public CreateFolderSharedDetailCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId cannot be empty");
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Invalid subscriber");
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Invalid folder Id");
            RuleFor(c => c.FolderId).NotEqual(0).WithMessage("Invalid folder Id");
        }
    }
}
