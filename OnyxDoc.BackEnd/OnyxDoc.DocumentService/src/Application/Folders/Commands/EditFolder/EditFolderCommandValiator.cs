using FluentValidation;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.EditFolder
{
    public class EditFolderCommandValiator : AbstractValidator<EditFolderCommand>
    {
        public EditFolderCommandValiator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name cannot be empty!");
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Id must be specified!");
        }
    }
}
