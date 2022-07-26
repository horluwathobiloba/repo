using FluentValidation;
using OnyxDoc.DocumentService.Application.Folders.Commands.CreateRootFolder;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateParentFolder
{
    public class CreateRootFolderCommandValidator : AbstractValidator<CreateRootFolderCommand>
    {
        public CreateRootFolderCommandValidator()
        {
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Invalid Id");
            RuleFor(c => c.ParentFolderId).NotEqual(0).WithMessage("Invalid Parent Folder Id");
            RuleFor(c => c.RootFolderId).NotEqual(0).WithMessage("Invalid Root Folder Id");

        }
    }
}
