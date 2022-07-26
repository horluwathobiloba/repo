using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Transactions.Queries
{
    public class GetTransactionByTransactionIdQuery : IRequest<Result>
    {
        public int TransactionId { get; set; }
    }

    public class GetTransactionByTransactionIdQueryQueryHandler : IRequestHandler<GetTransactionByTransactionIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionByTransactionIdQueryQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetTransactionByTransactionIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == request.TransactionId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Transaction details was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
