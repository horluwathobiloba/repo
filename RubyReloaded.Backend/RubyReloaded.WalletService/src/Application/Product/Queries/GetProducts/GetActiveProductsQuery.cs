using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductConfigurations.Queries.GetProducts
{
    public class GetActiveProductsQuery : IRequest<Result>
    {
    }

    public class GetActiveProductsQueryHandler : IRequestHandler<GetActiveProductsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetActiveProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Products.Where(x => x.Status == Domain.Enums.Status.Active).Include(x=>x.ProductItemCategories)
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Product by Id was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
