using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.Folders.Commands.CreateFolder
{
    public class CreateFoldersCommandValidator : AbstractValidator<CreateFoldersCommand>
    {
        public CreateFoldersCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("UserId cannot be empty");
            RuleFor(c => c.SubscriberId).NotEqual(0).WithMessage("Invalid subscriber");
            
            
            //TODo
            //Ensure Ids is not 0
        }
    }
}
