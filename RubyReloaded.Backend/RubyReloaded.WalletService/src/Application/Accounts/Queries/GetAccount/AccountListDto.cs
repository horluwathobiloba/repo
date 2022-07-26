
using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace RubyReloaded.WalletService.Application.Accounts.Queries
{
    public class AccountListDto : IMapFrom<Domain.Entities.Account>
{
        public int Id { get; set; }
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
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Account, AccountListDto> ();
            profile.CreateMap<AccountListDto, Domain.Entities.Account>();
        }
    }
}
