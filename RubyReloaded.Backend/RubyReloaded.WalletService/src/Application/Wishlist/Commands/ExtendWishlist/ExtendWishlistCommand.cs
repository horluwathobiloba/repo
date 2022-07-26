using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Commands.ExtendWishlist
{
    public class ExtendWishlistCommand: IRequest<Result>
    {
        public int WishlistId { get; set; }
        public decimal WishlistExtensionAmount { get; set; }
        public DateTime WishlistExtensionDate { get; set; }
        public ContributionFrequency WishlistExtensionFrequency { get; set; }
        public string UserId { get; set; }
    }

    public class ExtendWishlistCommandHandler : IRequestHandler<ExtendWishlistCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ExtendWishlistCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ExtendWishlistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Retrieve wishlist
                var existingWishlist = await _context.Wishlists.Where(a => a.Id == request.WishlistId && a.Status == Domain.Enums.Status.Active).FirstOrDefaultAsync();
                if (existingWishlist == null)
                {
                    return Result.Failure("Invalid Wish details");
                }
                //check if startdate has started , ask if wishlist affects start date
                if (existingWishlist.StartDate.Subtract(DateTime.Now).TotalDays > 0)
                {
                    return Result.Failure("You cannot extend a Wish which is yet to start");
                }
               
                //check if enddate is past
                if (request.WishlistExtensionDate.Subtract(existingWishlist.EndDate).TotalDays <= 0)
                {
                        return Result.Failure("Wish Start Date has elapsed");
                    
                }
                //Find out conditions for extension, can it be greater or less than normal created amounnt
                if (request.WishlistExtensionAmount <= 0)
                {
                    return Result.Failure("Invalid Wish Extension Amount");
                }
                //then extend else flag
                //TODO: Get other conditions for wishlist extension
                existingWishlist.EndDate = request.WishlistExtensionDate;
                existingWishlist.WishlistExtensionAmount = request.WishlistExtensionAmount;
                existingWishlist.LastModifiedDate = DateTime.Now;
                existingWishlist.LastModifiedBy = request.UserId;
                existingWishlist.WishlistExtensionDate = request.WishlistExtensionDate;
                existingWishlist.WishlistExtensionFrequency = request.WishlistExtensionFrequency;
                 _context.Wishlists.Update(existingWishlist);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Wishlist extended successfully",existingWishlist);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Extend Wishlist was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
