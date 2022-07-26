using FluentValidation;
using OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBook;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.UpdateAddressBook
{
    public class UpdateAddressBookCommandValidator : AbstractValidator<UpdateAddressBookCommand>
    {
        public UpdateAddressBookCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0).WithMessage(" recipient must be specified!");
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("A valid  must be specified!");
            RuleFor(v => v.EmailAddress).NotEmpty().WithMessage("A valid email must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
        }
    }
}
