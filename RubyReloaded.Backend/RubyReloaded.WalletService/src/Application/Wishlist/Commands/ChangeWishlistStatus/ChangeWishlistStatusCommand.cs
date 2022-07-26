using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Services.Commands.ChangeWishlistStatus
{
    public class ChangeWishlistStatusCommand: IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeWishlistStatusCommandHandler : IRequestHandler<ChangeWishlistStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeWishlistStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeWishlistStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                var wishlist = await _context.Wishlists.FindAsync(request.Id);
                if (wishlist == null)
                {
                    return Result.Failure(new string[] { "Invalid Wishlist details" });
                }
                switch (wishlist.Status)
                {
                    case Domain.Enums.Status.Active:
                        wishlist.Status = Domain.Enums.Status.Inactive;
                        wishlist.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Wishlist deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        wishlist.Status = Domain.Enums.Status.Active;
                        wishlist.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Wishlist activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        wishlist.Status = Domain.Enums.Status.Active;
                        wishlist.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Wishlist activation was successful";
                        break;
                    default:
                        break;
                }
                 _context.Wishlists.Update(wishlist);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Wishlist status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
