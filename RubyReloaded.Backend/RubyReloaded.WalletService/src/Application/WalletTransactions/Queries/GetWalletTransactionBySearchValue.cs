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

namespace RubyReloaded.WalletService.Application.WalletTransactions.Queries
{
    public class GetWalletTransactionBySearchValue : IRequest<Result>
    {
        public string SearchValue { get; set; }
    }

    public class GetWalletTransactionBySearchValueHandler : IRequestHandler<GetWalletTransactionBySearchValue, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWalletTransactionBySearchValueHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWalletTransactionBySearchValue request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.Where(x => x.PaymentChannel.Name.Contains(request.SearchValue)
                || x.TransactionTypeDesc.Contains(request.SearchValue)
                || x.AccountHolder.Contains(request.SearchValue))
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
