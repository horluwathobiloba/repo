using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.CreateCustomerAccount
{
    public class CreateCustomerAccountCommandValidator : AbstractValidator<CreateCustomerAccountCommand>
    {
        public CreateCustomerAccountCommandValidator()
        {
            RuleFor(v => v.Name) 
                .NotEmpty().NotNull().WithMessage("CustomerAccount Name must be specified");

            RuleFor(v => v.CustomerId)
                .NotEmpty().NotNull().WithMessage("Customer ID must be specified"); ;
        }
    }
}
