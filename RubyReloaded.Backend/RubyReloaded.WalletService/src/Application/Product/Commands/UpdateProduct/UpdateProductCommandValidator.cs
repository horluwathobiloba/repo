using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(v => v.Name)
               .NotEmpty().NotNull().WithMessage("Name is Required");

            RuleFor(v => v.Id)
               .NotEmpty().NotNull().WithMessage("Product Id is Required");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User Id is Required");

            RuleFor(v => v.MinimumFundingAmount)
                .GreaterThan(0).WithMessage("Minimum Funding Amount is Required");


            When(v => v.MinimumFundingAmount >= 0, () =>
            {
                RuleFor(v => v.MinimumFundingAmount).Equal(v => v.MaximumFundingAmount).WithMessage("Minimum funding amount cannot be same as Minimum funding amount!");
            });

            RuleFor(v => v.MaximumFundingAmount)
               .GreaterThan(0).WithMessage("Maximum Funding Amount is Required");

            When(v => v.MaximumFundingAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumFundingAmount).Equal(v => v.MinimumFundingAmount).WithMessage("Maximum funding amount cannot be same as Minimum funding amount!");
            });

            RuleFor(v => v.MinimumBalanceAmount)
               .GreaterThan(0).WithMessage("Minimum Balance Amount is Required");

            When(v => v.MinimumBalanceAmount >= 0, () =>
            {
                RuleFor(v => v.MinimumBalanceAmount).Equal(v => v.MaximumBalanceAmount).WithMessage("Minimum balance amount cannot be same as Minimum balance amount!");
            });


            RuleFor(v => v.MaximumBalanceAmount)
                .GreaterThan(0).WithMessage("Maximum Balance Amount is Required");

            When(v => v.MaximumBalanceAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumBalanceAmount).Equal(v => v.MinimumBalanceAmount).WithMessage("Maximum balance amount cannot be same as Minimum balance amount!");
            });

            RuleFor(v => v.MinimumWithdrawalAmount)
             .GreaterThan(0).WithMessage("Minimum Withdrawal Amount is Required");

            When(v => v.MinimumWithdrawalAmount >= 0, () =>
            {
                RuleFor(v => v.MinimumWithdrawalAmount).Equal(v => v.MaximumWithdrawalAmount).WithMessage("Minimum withdrawal amount cannot be same as Maximum withdrawal amount!");
            });

            RuleFor(v => v.MaximumWithdrawalAmount)
                .GreaterThan(0).WithMessage("Maximum Withdrawal Amount is Required");

            When(v => v.MaximumWithdrawalAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumWithdrawalAmount).Equal(v => v.MinimumWithdrawalAmount).WithMessage("Maximum withdrawal amount cannot be same as Minimum withdrawal amount!");
            });

            When(v => v.CommissionFeeType == Domain.Enums.FeeType.Percentage, () =>
            {
                RuleFor(v => v.CommissionAmountOrRate).GreaterThan(0).WithMessage("Commission rate must be greater than zero!")
                .LessThanOrEqualTo(100).WithMessage("Commission rate cannot be greater than hundred!");
            });

            When(v => v.CommissionFeeType == Domain.Enums.FeeType.FlatAmount, () =>
            {
                RuleFor(v => v.CommissionAmountOrRate).GreaterThan(0).WithMessage("Commission amount must be greater than zero!");
            });

            When(v => v.TransactionFeeType == Domain.Enums.FeeType.Percentage, () =>
            {
                RuleFor(v => v.TransactionFeeAmountOrRate).GreaterThan(0).WithMessage("Transaction fee rate must be greater than zero!")
                .LessThanOrEqualTo(100).WithMessage("Transaction fee rate cannot be greater than hundred!");
            });

            When(v => v.TransactionFeeType == Domain.Enums.FeeType.FlatAmount, () =>
            {
                RuleFor(v => v.TransactionFeeAmountOrRate).GreaterThan(0).WithMessage("Transaction fee amount must be greater than zero!");
            });


        }
    }
}
