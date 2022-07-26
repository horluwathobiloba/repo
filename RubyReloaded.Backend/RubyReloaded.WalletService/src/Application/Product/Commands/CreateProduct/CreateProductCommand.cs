using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
        public int MinimumHoldingPeriod { get; set; }
        public Duration MinimumHoldingDuration { get; set; }
        public int MaximumHoldingPeriod { get; set; }
        public Duration MaximumHoldingDuration { get; set; }
        public int GLSubClassAccountId { get; set; }
        public bool EnableCommission { get; set; }
        public FeeType CommissionFeeType { get; set; }
        public decimal CommissionAmountOrRate { get; set; }
        public bool EnableTransactionFee { get; set; }
        public FeeType TransactionFeeType { get; set; }
        public decimal TransactionFeeAmountOrRate { get; set; }
        public bool EnableInterest { get; set; }
        public bool EnablePenaltyCharges { get; set; }
        public FeeType PenaltyFeeType { get; set; }
        public decimal PenaltyFeeRate { get; set; }
        public string TermsAndConditions { get; set; }
        public List<ProductInterestRequest> ProductInterests { get; set; }
        public List<ProductFundingSourceRequest> ProductFundingSources { get; set; }
        public List<ProductSettlementAccountRequest> ProductSettlementAccounts { get; set; }
        public List<TransactionServiceRequest> TransactionServices { get; set; }
        public List<ProductItemCategoryRequest> ProductItemCategories { get; set; }

    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        public CreateProductCommandHandler(IApplicationDbContext context, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _base64ToFileConverter = base64ToFileConverter;
        }
        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Check to make sure we create just one cash product

                var cashProduct = await _context.Products.Where(a => a.ProductCategory == ProductCategory.Cash && request.ProductCategory == ProductCategory.Cash).FirstOrDefaultAsync();
                if (cashProduct != null)
                {
                    return Result.Failure(new string[] { "System can only have one cash Product" });
                }

                var productNameExists = await _context.Products.FirstOrDefaultAsync(x => x.Name == request.Name);
                if(productNameExists != null)
                {
                    return Result.Failure(new string[] { "Product already exist" });
                }
               
                await _context.BeginTransactionAsync();

                var product = new Product
                {
                    UserId = request.UserId,
                    EnablePenaltyCharges = request.EnablePenaltyCharges,
                    EnableCommission = request.EnableCommission,
                    EnableInterest = request.EnableInterest,
                    EnableTransactionFee = request.EnableTransactionFee,
                    TransactionFeeAmountOrRate = request.TransactionFeeAmountOrRate,
                    CommissionAmountOrRate = request.CommissionAmountOrRate,

                    CommissionFeeType = request.CommissionFeeType,
                    CommissionFeeTypeDesc = request.CommissionFeeType.ToString(),
                    TransactionFeeType = request.TransactionFeeType,
                    TransactionFeeTypeDesc = request.TransactionFeeType.ToString(),

                    MaximumBalanceAmount = request.MaximumBalanceAmount,
                    MinimumBalanceAmount = request.MinimumBalanceAmount,
                    MinimumFundingAmount = request.MinimumFundingAmount,
                    MaximumFundingAmount = request.MaximumFundingAmount,
                    MinimumWithdrawalAmount = request.MinimumWithdrawalAmount,
                    MaximumWithdrawalAmount = request.MaximumWithdrawalAmount,
                    EnableCustomerOverrideAmount = request.EnableCustomerOverrideAmount,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    Currency = request.Currency,
                    Description = request.Description,
                    ProductCategoryDesc = request.ProductCategory.ToString(),
                    GLSubClassAccountId = request.GLSubClassAccountId,
                    MaximumHoldingDuration = request.MaximumHoldingDuration,
                    MinimumHoldingDuration = request.MinimumHoldingDuration,
                    MaximumHoldingPeriod = request.MaximumHoldingPeriod,
                    MinimumHoldingPeriod = request.MinimumHoldingPeriod,
                    Name = request.Name,
                    PenaltyFeeRate = request.PenaltyFeeRate,
                    PenaltyFeeType = request.PenaltyFeeType,
                    ProductCategory = request.ProductCategory,
                    TransactionFee = request.TransactionFee,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    TermsAndConditions = request.TermsAndConditions
                };
                //Product Interest Configuration
                List<ProductInterest> productInterests = new List<ProductInterest>();
                foreach (var productInterest in request.ProductInterests)
                {
                    var interest = new ProductInterest
                    {
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.UserId,
                        Description = request.Description,
                        MaximumAmount = productInterest.MaximumAmount,
                        MinimumAmount = productInterest.MinimumAmount,
                        Rate = productInterest.Rate,
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                        VariableType = productInterest.VariableType,
                    };
                    List<ProductInterestPeriod> interestIntervals = new List<ProductInterestPeriod>();
                    if (productInterest.ProductInterestPeriod != null && productInterest.ProductInterestPeriod.Count > 0)
                    {
                        foreach (var productInterestPeriod in productInterest.ProductInterestPeriod)
                        {
                            var interestInterval = new ProductInterestPeriod
                            {
                                CreatedDate = DateTime.Now,
                                CreatedBy = request.UserId,
                                HoldingPeriod = productInterestPeriod.HoldingPeriod,
                                StatusDesc = Status.Active.ToString(),
                                Status = Status.Active
                            };
                            interestIntervals.Add(interestInterval);
                        }
                    }
                    productInterests.Add(interest);
                };

                product.ProductInterests = productInterests;

                //TransactionService Configuration
                List<TransactionService> TransactionServices = new List<TransactionService>();
                foreach (var TransactionService in request.TransactionServices)
                {
                    TransactionServices.Add(new TransactionService
                    {
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.UserId,
                        StatusDesc = Status.Active.ToString(),
                        TransactionServiceType = TransactionService.TransactionServiceType,
                        Name = TransactionService.TransactionServiceType.ToString(),
                        Status = Status.Active
                    });
                }
                product.TransactionServices = TransactionServices;

                //ProductSettlementAccount Configuration
                List<ProductSettlementAccount> productSettlementAccounts = new List<ProductSettlementAccount>();
                foreach (var settlementAccount in request.ProductSettlementAccounts)
                {
                    productSettlementAccounts.Add(new ProductSettlementAccount
                    {
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.UserId,
                        StatusDesc = Status.Active.ToString(),
                        AccountNumber = settlementAccount.AccountNumber,
                        BankId = settlementAccount.BankId,
                        Name = settlementAccount.BankName,
                        Status = Status.Active
                    });
                }
                product.ProductSettlementAccounts = productSettlementAccounts;

                //Funding Source Configuration
                List<ProductFundingSource> productFundingSources = new List<ProductFundingSource>();
                foreach (var productFundingSource in request.ProductFundingSources)
                {
                    productFundingSources.Add(new ProductFundingSource
                    {
                        PaymentChannelId = productFundingSource.PaymentChannelId,
                        ProductFundingCategory = productFundingSource.ProductFundingCategory,
                        ProductId = productFundingSource.ProductId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.UserId,
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active
                    });
                }
                /*product.ProductFundingSources = productFundingSources;*/

                //Product Item Category
                List<ProductItemCategory> productItemCategories = new List<ProductItemCategory>();
                foreach (var productItemCategory in request.ProductItemCategories)
                {
                    productItemCategories.Add(new ProductItemCategory
                    {
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedByEmail = request.UserId,
                        StatusDesc = Status.Active.ToString(),
                        Status = Status.Active,
                         Name = productItemCategory.Name,
                         DefaultImageUrl = await _base64ToFileConverter.ConvertBase64StringToFile(productItemCategory.DefaultImageUrl, productItemCategory.Name + "_" + DateTime.Now.Ticks + ".png"),
                        Description = productItemCategory.Description,
                    });
                }
                product.ProductItemCategories = productItemCategories;

                await _context.Products.AddAsync(product);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Products created successfully",product);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Product creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}