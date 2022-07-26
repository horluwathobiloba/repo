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

namespace RubyReloaded.WalletService.Application.Products.Commands.UpdateWishlist
{
    public class UpdateWishlistCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public WishCategory WishCategory { get; set; }
        public ContributionFrequency ContributionFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InterestRate { get; set; }
        public int AccountId { get; set; }
        public WishListStatus WishListStatus { get; set; }
        public string WishListStatusDesc { get; set; }
        public decimal TargetAmount { get; set; }
        public string ImageUrl { get; set; }
        public decimal WishlistBalance { get; set; }
        public string WishlistSavingsPeriod { get; set; }
        public decimal WishlistExtensionAmount { get; set; }
        public WishlistFundingSourceRequest WishlistFundingSource { get; set; }
        public DateTime WishlistExtensionDate { get; set; }
        public ContributionFrequency WishlistExtensionFrequency { get; set; }
        public int DaysLeft { get; set; }
        public decimal RecurringContribution { get; set; }
    }

    public class UpdateWishlistCommandHandler : IRequestHandler<UpdateWishlistCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateWishlistCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateWishlistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingWishlist = await _context.Wishlists.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (existingWishlist == null)
                {
                    return Result.Failure(new string[] { "Product  Id not found " });
                }

                existingWishlist.Name = request.Name;
                existingWishlist.TargetAmount = request.TargetAmount;
                existingWishlist.ContributionFrequency = request.ContributionFrequency;
                existingWishlist.DaysLeft = request.DaysLeft;
                existingWishlist.StartDate = request.StartDate;
                existingWishlist.EndDate = request.EndDate;
                existingWishlist.InterestRate = request.InterestRate;
                existingWishlist.ImageUrl = request.ImageUrl;
                existingWishlist.WishCategory = request.WishCategory;
                existingWishlist.RecurringContribution = request.RecurringContribution;
                existingWishlist.WishlistBalance = request.WishlistBalance;
                existingWishlist.WishlistSavingsPeriod = request.WishlistSavingsPeriod;
                existingWishlist.WishListStatus = WishListStatus.InProgress;
                existingWishlist.WishListStatusDesc = WishListStatus.InProgress.ToString();
                existingWishlist.Description = request.Description;
                existingWishlist.StatusDesc = Status.Active.ToString();
                existingWishlist.Status = Status.Active;
                existingWishlist.LastModifiedDate = DateTime.Now;
                existingWishlist.WishlistFundingSource = new WishlistFundingSource
                {
                    FundingSource = request.WishlistFundingSource.FundingSource,
                    FundingSourceCategory = request.WishlistFundingSource.FundingSourceCategory,
                    PaymentChannelId = request.WishlistFundingSource.PaymentChannelId
                };

                _context.Wishlists.Update(existingWishlist);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Wishlists updated successfully", existingWishlist);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Wishlist Update was not successful ", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
