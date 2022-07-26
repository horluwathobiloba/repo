using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Products.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Products was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
