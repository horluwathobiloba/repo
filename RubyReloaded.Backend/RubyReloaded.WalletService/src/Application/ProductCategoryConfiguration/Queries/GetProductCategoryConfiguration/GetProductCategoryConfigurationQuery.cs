using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Queries.GetProductCategoryConfiguration
{
    public class GetProductCategoryConfigurationQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetProductCategoryConfigurationQueryHandler : IRequestHandler<GetProductCategoryConfigurationQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetProductCategoryConfigurationQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetProductCategoryConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.ProductCategoryConfigurations.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Product Category Configuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
