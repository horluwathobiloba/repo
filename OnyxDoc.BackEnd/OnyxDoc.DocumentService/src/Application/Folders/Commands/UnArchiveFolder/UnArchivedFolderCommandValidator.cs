using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.UnArchiveFolder
{
    public class UnArchivedFolderCommandValidator : AbstractValidator<UnArchiveFolderCommand>
    {
        public UnArchivedFolderCommandValidator()
        {
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.Id).NotEqual(0).WithMessage("Id must be specified!");
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
