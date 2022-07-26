using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.DuplicateFolder
{
    public class DuplicateFolderCommandValidator : AbstractValidator<DuplicateFolderCommand>
    {
        public DuplicateFolderCommandValidator()
        {
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
