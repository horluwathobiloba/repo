using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.Customer.UpdateAccount
{
    public class UpdateCustomerAccountCommandValidator : AbstractValidator<UpdateCustomerAccountCommand>
    {
        public UpdateCustomerAccountCommandValidator()
        {
            RuleFor(v => v.Name) 
                .NotEmpty().NotNull().WithMessage("Account Name must be specified");

            RuleFor(v => v.AccountType)
             .NotEmpty().NotNull().WithMessage("Account Type must be specified");

            RuleFor(v => v.AccountClass)
            .NotEmpty().NotNull().WithMessage("Account Class must be specified");

            RuleFor(v => v.UserId)
                .NotEmpty().NotNull().WithMessage("UserId must be specified"); ;
        }
    }
}
