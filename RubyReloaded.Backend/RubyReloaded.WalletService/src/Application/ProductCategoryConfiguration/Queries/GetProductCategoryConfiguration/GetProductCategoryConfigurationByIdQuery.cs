using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.ProductCategoryConfigurations.Queries.GetProductCategoryConfiguration
{
    public class GetProductCategoryConfigurationByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int ProductCategoryConfigurationId { get; set; }
    }

    public class GetProductCategoryConfigurationByIdQueryHandler : IRequestHandler<GetProductCategoryConfigurationByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetProductCategoryConfigurationByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetProductCategoryConfigurationByIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var result = await _context.ProductCategoryConfigurations.FirstOrDefaultAsync(x => x.Id == request.ProductCategoryConfigurationId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Product Category Configuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
