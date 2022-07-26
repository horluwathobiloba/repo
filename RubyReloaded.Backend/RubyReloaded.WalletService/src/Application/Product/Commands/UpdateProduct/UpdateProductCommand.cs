using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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
        public int MinimumHoldingPeriod { get; set; }
        public Duration MinimumHoldingDuration { get; set; }
        public int MaximumHoldingPeriod { get; set; }
        public Duration MaximumHoldingDuration { get; set; }
        public int GLSubClassAccountId { get; set; }
        public int ProductInterestId { get; set; }
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
        public ICollection<ProductInterest> ProductInterests { get; set; }
        public List<ProductFundingSourceRequest> ProductFundingSources { get; set; }
        public List<ProductSettlementAccountRequest> ProductSettlementAccounts { get; set; }
        public List<TransactionServiceRequest> TransactionServices { get; set; }
        //public List<ProductItemCategoryRequest> ProductItemCategories { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        public UpdateProductCommandHandler(IApplicationDbContext context, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _base64ToFileConverter = base64ToFileConverter;
        }
        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Products.Include(a=>a.ProductFundingSources).Include(a=>a.ProductSettlementAccounts)
                             .Include(a=>a.TransactionServices).Include(a=>a.ProductInterests).Include(a=>a.ProductItemCategories).FirstOrDefaultAsync(x => x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Product  Id not found " });
                }

                entity.Name = request.Name;
                entity.MinimumFundingAmount = request.MinimumFundingAmount;
                entity.MaximumFundingAmount = request.MaximumFundingAmount;
                entity.MinimumBalanceAmount = request.MinimumBalanceAmount;
                entity.MaximumBalanceAmount = request.MaximumBalanceAmount;
                entity.MinimumWithdrawalAmount = request.MinimumWithdrawalAmount;
                entity.MaximumWithdrawalAmount = request.MaximumWithdrawalAmount;
                entity.EnableCustomerOverrideAmount = request.AllowCustomerOverrideAmount;
                entity.MaximumHoldingDuration = request.MaximumHoldingDuration;
                entity.MaximumHoldingPeriod = request.MaximumHoldingPeriod;
                entity.GLSubClassAccountId = request.GLSubClassAccountId;
                entity.MinimumHoldingPeriod = request.MinimumHoldingPeriod;
                entity.MinimumHoldingDuration = request.MinimumHoldingDuration;
                entity.PenaltyFeeRate = request.PenaltyFeeRate;
                entity.PenaltyFeeType = request.PenaltyFeeType;
                entity.ProductCategoryDesc = request.ProductCategory.ToString();
                entity.ProductCategory = request.ProductCategory;

                entity.EnablePenaltyCharges = request.EnablePenaltyCharges;
                entity.EnableCommission = request.EnableCommission;
                entity.EnableInterest = request.EnableInterest;
                entity.EnableTransactionFee = request.EnableTransactionFee;

                entity.TransactionFeeType = request.TransactionFeeType;
                entity.TransactionFeeTypeDesc = request.TransactionFeeType.ToString();
               

                entity.TransactionFee = request.TransactionFee;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = request.UserId;
                entity.Currency = request.Currency;
                entity.Description = request.Description;
                entity.StatusDesc = Status.Active.ToString();
                entity.Status = Status.Active;
                //TODO fix edit product
                if (request.ProductInterests != null)
                {
                    if (entity.ProductInterests != null || entity.ProductInterests.Count > 0)
                    {
                        foreach (var productInterest in request.ProductInterests)
                        {
                            if (true)
                            {

                            }
                        }
                        entity.ProductInterests = request.ProductInterests;
                    }
                  
                }

                _context.Products.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Product  Update was not successful ", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
