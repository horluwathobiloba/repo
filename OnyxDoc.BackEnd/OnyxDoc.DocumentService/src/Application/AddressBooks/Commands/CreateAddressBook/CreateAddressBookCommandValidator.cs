using FluentValidation;
using OnyxDoc.DocumentService.Application.Recipients.Commands.CreateRecipient;
using OnyxDoc.DocumentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.DocumentService.Application.AddressBooks.Commands.CreateAddressBook
{
    public class CreateAddressBookCommandValidator:AbstractValidator<CreateAddressBookCommand>
    {
        public CreateAddressBookCommandValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User Id must be specified!"); 
        }

    }
}
