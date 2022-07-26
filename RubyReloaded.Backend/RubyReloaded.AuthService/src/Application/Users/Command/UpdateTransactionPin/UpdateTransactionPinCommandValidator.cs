using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdateTransactionPin
{
    public class UpdateTransactionPinCommandValidator: AbstractValidator<UpdateTransactionPinCommand>
    {
        public UpdateTransactionPinCommandValidator()
        {

            RuleFor(v => v.UserId)
                 .MaximumLength(200)
                 .NotEmpty();  
            RuleFor(v => v.Pin)
                 .MaximumLength(200)
                 .NotEmpty();
        }
    }
}
