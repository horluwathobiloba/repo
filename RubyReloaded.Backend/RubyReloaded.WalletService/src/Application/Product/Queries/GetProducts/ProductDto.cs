using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetProducts
{
    public class ProductDto : IMapFrom<Domain.Entities.Product>
    {
        public decimal MinimumFundingAmount { get; set; }
        public decimal MaximumFundingAmount { get; set; }
        public decimal MinimumBalanceAmount { get; set; }
        public decimal MaximumBalanceAmount { get; set; }
        public decimal MinimumWithdrawalAmount { get; set; }
        public decimal MaximumWithdrawalAmount { get; set; }
        public bool AllowCustomerOverrideAmount { get; set; }
        public string Currency { get; set; }
        public decimal TransactionFee { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string ProductCategoryDesc { get; set; }
        public int MinimumHoldingPeriod { get; set; }
        public Duration MinimumHoldingDuration { get; set; }
        public int MaximumHoldingPeriod { get; set; }
        public Duration MaximumHoldingDuration { get; set; }
        public Domain.Entities.Account GLSubClassAccount { get; set; }
        public int GLSubClassAccountId { get; set; }
        public ICollection<ProductInterest> ProductInterest { get; set; }
        public bool ComputeCommissionFee { get; set; }
        public bool ComputeTransactionFee { get; set; }
        public bool ComputeInterest { get; set; }
        public bool AllowPenaltyCharges { get; set; }
        public FeeType PenaltyFeeType { get; set; }
        public decimal PenaltyFeeRate { get; set; }
        public string TermsAndConditions { get; set; }
        public ICollection<TransactionService> TransactionServices { get; set; }
        public ICollection<ProductFundingSource> ProductFundingSources { get; set; }
        public ICollection<ProductSettlementAccount> ProductSettlementAccounts { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Product, ProductDto>();
            profile.CreateMap<ProductDto, Domain.Entities.Product>();

            profile.CreateMap<Domain.Entities.TransactionService, TransactionServiceDto>();
            profile.CreateMap<TransactionServiceDto, Domain.Entities.TransactionService>();


            profile.CreateMap<Domain.Entities.ProductFundingSource, ProductFundingSourceDto>();
            profile.CreateMap<ProductFundingSourceDto, Domain.Entities.ProductFundingSource>();

            profile.CreateMap<Domain.Entities.ProductSettlementAccount, ProductSettlementAccountDto>();
            profile.CreateMap<ProductSettlementAccountDto, Domain.Entities.ProductSettlementAccount>();


        }
    }
}
