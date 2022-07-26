using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WalletTransactions.Queries
{
    public class GetWalletTransactionByTransactionIdQuery : IRequest<Result>
    {
        public int TransactionId { get; set; }
    }

    public class GetWalletTransactionByTransactionIdQueryQueryHandler : IRequestHandler<GetWalletTransactionByTransactionIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletTransactionByTransactionIdQueryQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletTransactionByTransactionIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.FirstOrDefaultAsync(x => x.Id == request.TransactionId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Transaction details was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
