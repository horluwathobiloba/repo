using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetWishlist
{
    public class GetActiveWishlistsQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetActiveWishlistsQueryHandler : IRequestHandler<GetActiveWishlistsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetActiveWishlistsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetActiveWishlistsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Wishlists.Where(x=>x.UserId == request.UserId && x.Status == Domain.Enums.Status.Active).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving active wishlists was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
