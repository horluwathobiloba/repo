using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletTransactionsQuery : IRequest<Result>
    {
       
    }

    public class GetWalletTransactionsQueryHandler : IRequestHandler<GetWalletTransactionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletTransactionsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.ToListAsync();
                return Result.Success( result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Transactions was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
