using AutoMapper;
using RubyReloaded.WalletService.Application.Common.Mappings;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetProducts
{
    public class ProductSettlementAccountDto : IMapFrom<Domain.Entities.ProductSettlementAccount>
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public int BankId { get; set; }
        public int ProductId { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
           
            profile.CreateMap<Domain.Entities.ProductSettlementAccount, ProductSettlementAccountDto>();
            profile.CreateMap<ProductSettlementAccountDto, Domain.Entities.ProductSettlementAccount>();


        }
    }
}
