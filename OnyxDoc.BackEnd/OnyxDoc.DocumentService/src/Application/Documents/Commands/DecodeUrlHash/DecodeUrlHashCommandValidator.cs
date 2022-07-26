using FluentValidation; 

namespace OnyxDoc.DocumentService.Application.Commands.DecodeUrlHash
{
    public class DecodeUrlHashCommandValidator : AbstractValidator<DecodeUrlHashCommand>
    {
        public DecodeUrlHashCommandValidator()
        { 
           
            RuleFor(v => v.DocumentLinkHash).NotEmpty().WithMessage("Document Link Hash must be specified!");
        }
    }
}
