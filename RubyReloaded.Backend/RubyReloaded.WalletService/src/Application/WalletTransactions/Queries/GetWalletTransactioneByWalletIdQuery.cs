using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetWalletTransactioneByWalletIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int WalletId { get; set; }
        public bool AccessLevel { get; set; }
    }

    public class GetWalletTransactioneByWalletIdQueryHandler : IRequestHandler<GetWalletTransactioneByWalletIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletTransactioneByWalletIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletTransactioneByWalletIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.Where(x => x.WalletId == request.WalletId).ToListAsync();
                return Result.Success( result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
