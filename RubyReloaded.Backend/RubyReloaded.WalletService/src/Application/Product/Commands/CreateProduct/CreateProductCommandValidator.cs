using FluentValidation;

namespace RubyReloaded.WalletService.Application.Products.Commands.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(v => v.Name)
               .NotEmpty().NotNull().WithMessage("Name is Required");

            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User Id is Required");

            RuleFor(v => v.MinimumFundingAmount)
                .GreaterThan(0).WithMessage("Minimum Funding Amount is Required");


            When(v => v.MinimumFundingAmount >= 0 ,() =>
            {
                RuleFor(v => v.MinimumFundingAmount).LessThan(v => v.MaximumFundingAmount).WithMessage("Maximum funding amount cannot be more than Minimum funding amount!");
            });

            RuleFor(v => v.MaximumFundingAmount)
               .GreaterThan(0).WithMessage("Maximum Funding Amount is Required");

            When(v => v.MaximumFundingAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumFundingAmount).GreaterThan(v => v.MinimumFundingAmount).WithMessage("Maximum funding amount cannot be less than Minimum funding amount!");
            });

            RuleFor(v => v.MinimumBalanceAmount)
               .GreaterThan(0).WithMessage("Minimum Balance Amount is Required");

            When(v => v.MinimumBalanceAmount >= 0, () =>
            {
                RuleFor(v => v.MinimumBalanceAmount).LessThan(v => v.MaximumBalanceAmount).WithMessage("Minimum balance amount cannot be more than Maximum balance amount!");
            });

           
            RuleFor(v => v.MaximumBalanceAmount)
                .GreaterThan(0).WithMessage("Maximum Balance Amount is Required");

            When(v => v.MaximumBalanceAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumBalanceAmount).GreaterThan(v => v.MinimumBalanceAmount).WithMessage("Maximum balance amount must be greater than Minimum balance amount!");
            });

            RuleFor(v => v.MinimumWithdrawalAmount)
             .GreaterThan(0).WithMessage("Minimum Withdrawal Amount is Required");

            When(v => v.MinimumWithdrawalAmount >= 0, () =>
            {
                RuleFor(v => v.MinimumWithdrawalAmount).LessThan(v => v.MaximumWithdrawalAmount).WithMessage("Minimum withdrawal amount cannot be more than the Maximum withdrawal amount!");
            });

            RuleFor(v => v.MaximumWithdrawalAmount)
                .GreaterThan(0).WithMessage("Maximum Withdrawal Amount is Required");

            When(v => v.MaximumWithdrawalAmount >= 0, () =>
            {
                RuleFor(v => v.MaximumWithdrawalAmount).GreaterThan(v => v.MinimumWithdrawalAmount).WithMessage("Maximum withdrawal amount cannot be lower than Minimum withdrawal amount!");
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
