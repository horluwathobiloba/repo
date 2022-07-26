using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.MoveFolder
{
    public class MoveFolderCommandValidator : AbstractValidator<MoveFolderCommand>
    {
        public MoveFolderCommandValidator()
        {
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.RootFolderId).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.ParentFolderId).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
