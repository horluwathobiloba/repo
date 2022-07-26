using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetProducts
{
    public class GetWishlistsQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetWishlistsQueryHandler : IRequestHandler<GetWishlistsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWishlistsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetWishlistsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Wishlists.Where(x => x.UserId == request.UserId).ToListAsync();
                /*var result = await _context.Wishlists.ToListAsync();*/
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wishlists was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
