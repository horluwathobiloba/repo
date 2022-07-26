using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Command.UpdateBVN
{
    public class UpdateBVNCommandValidator: AbstractValidator<UpdateBVNCommand>
    {
        public UpdateBVNCommandValidator()
        {

            RuleFor(v => v.UserId)
                 .MaximumLength(200)
                 .NotEmpty()
                 .WithMessage("Please input valid UserId");  
            RuleFor(v => v.BVN)
                 .MaximumLength(200)
                 .NotEmpty()
                 .WithMessage("Please input valid BVN");
            
        }
    }
}
