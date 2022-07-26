using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductConfigurations.Queries.GetProducts
{
    public class GetWishlistByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetWishlistByIdQueryHandler : IRequestHandler<GetWishlistByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWishlistByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWishlistByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Wishlists.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wishlist by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
