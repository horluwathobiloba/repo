using FluentValidation;  

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBookStatus
{
    public class UpdateAddressBookStatusCommandValidator : AbstractValidator<UpdateAddressBookStatusCommand>
    {
        public UpdateAddressBookStatusCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Address Book Id must be specified!");
        }
    }
}
