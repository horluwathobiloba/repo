using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccountConfigurations.Queries.GetVirtualAccountConfiguration
{
    public class GetVirtualAccountConfigurationQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetVirtualAccountConfigurationQueryHandler : IRequestHandler<GetVirtualAccountConfigurationQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetVirtualAccountConfigurationQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetVirtualAccountConfigurationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.VirtualAccountConfigurations.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Virtual Account Configuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
