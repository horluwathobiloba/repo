using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using RubyReloaded.WalletService.Domain.Common;
using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Domain.Entities 
{
    public class Account : AuditableEntity
    {
        public string AccountNumber { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public string AccountStatusDesc { get; set; }
        public AccountType AccountType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public int ParentAccountId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal DebitBalanceLimit { get; set; }
        public decimal CreditBalanceLimit { get; set; }
        public decimal MinimumFundingAmount { get; set; }
        public decimal MaximumFundingAmount { get; set; }
        public decimal MinimumWithdrawalAmount { get; set; }
        public decimal MaximumWithdrawalAmount { get; set; }
        public AccountFreezeType AccountFreezeType { get; set; } 
        public AccountClass AccountClass { get; set; }
        public int AccountPrefix { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
       
    }
}
