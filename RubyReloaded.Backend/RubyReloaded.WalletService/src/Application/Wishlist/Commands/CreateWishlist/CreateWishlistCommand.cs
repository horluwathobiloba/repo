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

namespace RubyReloaded.WalletService.Application.Products.Commands.CreateWishlist
{
    public class CreateWishlistCommand : IRequest<Result>
    {
        public int ProductId { get; set; }
        public string Name  { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public WishCategory WishCategory { get; set; }
        public ContributionFrequency ContributionFrequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InterestRate { get; set; }
        public WishListStatus WishListStatus { get; set; }
        public WishlistFundingSourceRequest WishlistFundingSource { get; set; }
        public decimal TargetAmount { get; set; }
        public string WishlistSavingsPeriod { get; set; }
        public string ImageUrl { get; set; }

    }

    public class CreateWishlistCommandHandler : IRequestHandler<CreateWishlistCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        public CreateWishlistCommandHandler(IApplicationDbContext context, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _base64ToFileConverter = base64ToFileConverter;
        }
        public async Task<Result> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingWishlist = await _context.Wishlists.Where(a => a.ProductId == request.ProductId && a.UserId == request.UserId && a.Name == request.Name).FirstOrDefaultAsync();
                if (existingWishlist != null)
                {
                    return Result.Failure("Wishlist Details already exist");
                }
                //TODO: Get existing user and get accountid tied to product,then create wishlist account for user

                //var account = await _context.Accounts.Where(a=>a.ProductId == request.ProductId && a.Status == Status.Active).FirstOrDefaultAsync();
                //if (account == null)
                //{
                //    return Result.Failure("Invalid Product Account Ids");
                //}
                //Product ID
                //TODO - Check customer override

                await _context.BeginTransactionAsync();
                //TODO: FUNCTION TO DEBIT CUSTOMER PERIODICALLY
                var wishlist = new Wishlist
                {
                    UserId = request.UserId,
                    ProductId = request.ProductId,
                    TargetAmount = request.TargetAmount,
                    ContributionFrequency = request.ContributionFrequency,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    InterestRate = request.InterestRate,
                    ImageUrl = await _base64ToFileConverter.ConvertBase64StringToFile(request.ImageUrl, request.Name + "_" + DateTime.Now.Ticks + ".png"),
                    WishCategory = request.WishCategory,
                    //RecurringContribution = request.RecurringContribution,
                  
                    WishListStatus = WishListStatus.InProgress,
                    WishListStatusDesc = WishListStatus.InProgress.ToString(),
                    Description = request.Description,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    Name = request.Name,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                     
                };
                wishlist.WishlistFundingSource = new WishlistFundingSource
                {
                    FundingSource = request.WishlistFundingSource.FundingSource,
                    FundingSourceCategory = request.WishlistFundingSource.FundingSourceCategory,
                    PaymentChannelId = request.WishlistFundingSource.PaymentChannelId
                };
                //TODO:Recurring Contribution
                await _context.Wishlists.AddAsync(wishlist);
                var res = await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Wishlist created successfully", wishlist);
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Wishlist creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
