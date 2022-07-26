using FluentValidation;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Commands.CreatePaymentChannel
{
    public class CreatePaymentChannelCommandValidator : AbstractValidator<CreatePaymentChannelCommand>
    {
        public CreatePaymentChannelCommandValidator()
        {
            RuleFor(v => v.Name).MaximumLength(200).NotEmpty().WithMessage("PaymentChannel name cannot be empty and cannot exceed 200 characters!");
            RuleFor(v => v.CurrencyConfigurationId).NotNull().NotEmpty().WithMessage("Currency code cannot be null!");
            RuleFor(v => v.TransactionFeeType.ToInt()).NotEqual(0).WithMessage("FeeType must be specified!");
            //RuleFor(v => v).Must(ValidFeeType).WithMessage("Invalid entries for the specified FeeType");

            // RuleFor(x => x.MyAccountNumber).Must(AccountNo_Length).WithMessage("Account number Length must be equal or greater than 8");
            //RuleFor(x => x.Name).NotEmpty().WithMessage("Employee Name is required");
            //RuleFor(x => x.Mail_ID).NotEmpty().WithMessage("Employee Mail_ID is required");
            //RuleFor(x => x.DOB).NotEmpty().WithMessage("Employee DOB is required");
            //RuleFor(x => x.Password).NotEmpty().WithMessage("Employee Password is required");
            //RuleFor(x => x.Confirm_Password).NotEmpty().WithMessage("Employee Password is required");
            //RuleFor(x => x.Confirm_Password).Equal(x => x.Password).WithMessage("Password Don't Match");
            //RuleFor(x => x.Mail_ID).EmailAddress().WithMessage("Email Address is not correct");
            //RuleFor(x => x.DOB).Must(Validate_Age).WithMessage("Age Must be 18 or Greater");
        }
    }
}