using FluentValidation; 
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.DeleteAddressBook
{
    public class DeleteAddressBookCommandValidator : AbstractValidator<DeleteAddressBookCommand>
    {
        public DeleteAddressBookCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.Id).GreaterThan(0).WithMessage(" recipient must be specified!"); 
        }
    }
}
