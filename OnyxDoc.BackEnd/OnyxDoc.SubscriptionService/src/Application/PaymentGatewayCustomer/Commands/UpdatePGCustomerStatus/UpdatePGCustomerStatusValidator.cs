using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    class UpdatePGCustomerStatusValidator : AbstractValidator<UpdatePGCustomerStatusCommand>
    {
        public UpdatePGCustomerStatusValidator()
        {
            RuleFor(v => v.SubscriberId).GreaterThan(0).WithMessage("Subscriber identifier must be specified!"); 
            RuleFor(v => v.Id).GreaterThan(0).WithMessage("Payment gateway customer identifier must be specified!");
            RuleFor(v => v.UserId).NotEmpty().WithMessage("User id must be specified!");
            RuleFor(v => v.Status).IsInEnum().WithMessage("Status must be specified!");
        }
    }
}
