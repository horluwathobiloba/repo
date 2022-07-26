using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductConfigurations.Queries.GetProducts
{
    public class GetWishlistByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int WishlistId { get; set; }
    }

    public class GetWishlistByUserIdQueryHandler : IRequestHandler<GetWishlistByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWishlistByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWishlistByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Wishlists.FirstOrDefaultAsync(x => x.Id == request.WishlistId && x.Status == Domain.Enums.Status.Active);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wishlist by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
