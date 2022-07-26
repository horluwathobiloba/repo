using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletOpeningBalanceQuery : IRequest<Result>
    {
        public string WalletAccountNumber { get; set; }
        public string UserId { get; set; }
    }
    public class GetWalletOpeningBalanceQueryHandler : IRequestHandler<GetWalletOpeningBalanceQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletOpeningBalanceQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Result> Handle(GetWalletOpeningBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.UserId == request.UserId && a.WalletAccountNumber == request.WalletAccountNumber);
                if (wallet == null)
                {
                    return Result.Failure("Invalid Wallet Account Number");
                }
                return Result.Success( wallet.OpeningBalance);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Opening Balance was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
