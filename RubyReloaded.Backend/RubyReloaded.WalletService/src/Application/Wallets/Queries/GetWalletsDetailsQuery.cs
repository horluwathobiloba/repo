using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletsDetailsQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetWalletDetailsQueryHandler : IRequestHandler<GetWalletsDetailsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletDetailsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletsDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallets = await _context.Wallets.Where(a => a.UserId == request.UserId).ToListAsync();
                if (wallets== null)
                {
                    return Result.Failure("Invalid Wallet Account Number");
                }

                return Result.Success(wallets);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
