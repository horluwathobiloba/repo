using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder
{
    public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
    {
        public CreateFolderCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Invalid Id");
            RuleFor(c => c.RootFolderId).NotEqual(0).NotEmpty().WithMessage("Invalid Root Folder Id");
            RuleFor(c => c.ParentFolderId).NotEqual(0).NotEmpty().WithMessage("Invalid Parent Folder Id");

            //TODo
            //Ensure Ids is not 0
        }
    }
}
