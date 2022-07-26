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
    public class GetWalletTransactioneByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
      
    }

    public class GetWalletTransactioneByUserIdQueryHandler : IRequestHandler<GetWalletTransactioneByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletTransactioneByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletTransactioneByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.Where(x=> x.CreatedBy == request.UserId).ToListAsync();
                return Result.Success( result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
