using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Transactions.Queries
{
    public class TransactionDto : IMapFrom<Domain.Entities.Transaction>
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionServiceType TransactionCategory { get; set; }
        public string TransactionCategoryDesc { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public string TransactionStatusDesc { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        public Domain.Entities.Account Account { get; set; }
        public string TransactionReference { get; set; }
        public string ExternalTransactionReference { get; set; }
        public string RecipientName { get; set; }
        public string Narration { get; set; }
        public string RecipientProfilePicture { get; set; }
        public int PaymentChannelId { get; set; }
        public PaymentChannel PaymentChannel { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Transaction, TransactionDto>();
            profile.CreateMap<TransactionDto, Transaction>();
        }
    }
}
