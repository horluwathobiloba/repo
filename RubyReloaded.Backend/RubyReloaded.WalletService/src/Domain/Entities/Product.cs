using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;
using System.Collections.Generic;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class Product   : AuditableEntity
    {
        public decimal MinimumFundingAmount { get; set; }
        public decimal MaximumFundingAmount { get; set; }
        public decimal MinimumBalanceAmount { get; set; }
        public decimal MaximumBalanceAmount { get; set; }
        public decimal MinimumWithdrawalAmount { get; set; }
        public decimal MaximumWithdrawalAmount { get; set; }
        public bool EnableCustomerOverrideAmount { get; set; }
        public string Currency { get; set; }
        public decimal TransactionFee { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductCategoryDesc { get; set; }
        public int MinimumHoldingPeriod { get; set; }
        public Duration MinimumHoldingDuration { get; set; }
        public int MaximumHoldingPeriod { get; set; }
        public Duration MaximumHoldingDuration { get; set; }
        public Account GLSubClassAccount { get; set; }
        public int GLSubClassAccountId { get; set; }
        public ICollection<ProductInterest> ProductInterests { get; set; }
        public bool EnableCommission { get; set; }
        public FeeType CommissionFeeType { get; set; }
        public string CommissionFeeTypeDesc { get; set; }
        /// <summary>
        /// If fee type is set to percentage, the value entered is a Percentage value 
        /// else if fee type is Amount,the value entered is an Amount
        /// </summary>
        public decimal CommissionAmountOrRate { get; set; }
        public bool EnableTransactionFee { get; set; }
        public FeeType TransactionFeeType { get; set; }
        public string TransactionFeeTypeDesc { get; set; }
        public decimal TransactionFeeAmountOrRate{ get; set; }
        public bool EnableInterest { get; set; }
        public bool EnablePenaltyCharges { get; set; }
        public FeeType PenaltyFeeType { get; set; }
        public decimal PenaltyFeeRate { get; set; }
        public string TermsAndConditions { get; set; } 
        public ICollection<TransactionService> TransactionServices { get; set; }
        public ICollection<ProductFundingSource> ProductFundingSources { get; set; }
        public ICollection<ProductSettlementAccount> ProductSettlementAccounts { get; set; }
        public ICollection<ProductItemCategory> ProductItemCategories { get; set; }
    }

}
