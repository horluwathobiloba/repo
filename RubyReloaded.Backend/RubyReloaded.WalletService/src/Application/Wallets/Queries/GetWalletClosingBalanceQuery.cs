using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletClosingBalanceQuery : IRequest<Result>
    {
        public string WalletAccountNumber { get; set; }
        public string UserId { get; set; }
    }
    public class GetWalletClosingBalanceQueryHandler : IRequestHandler<GetWalletClosingBalanceQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletClosingBalanceQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetWalletClosingBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.WalletAccountNumber == request.WalletAccountNumber);
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Account Number");
                }
                return Result.Success( wallet.ClosingBalance);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Closing Balance was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
