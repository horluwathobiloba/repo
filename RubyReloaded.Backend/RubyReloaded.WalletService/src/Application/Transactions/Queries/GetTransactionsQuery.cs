using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Wallets.Queries
{
    public class GetTransactionsQuery : IRequest<Result>
    {
       
    }

    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Transactions.ToListAsync();
                return Result.Success( result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Transactions was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }


}
