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

namespace RubyReloaded.WalletService.Application.Transactions.Queries
{
    public class GetTransactionBySearchValue : IRequest<Result>
    {
        public string SearchValue { get; set; }
    }

    public class GetTransactionBySearchValueHandler : IRequestHandler<GetTransactionBySearchValue, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetTransactionBySearchValueHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetTransactionBySearchValue request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Transactions.Where(x => x.PaymentChannel.Name.Contains(request.SearchValue)
                || x.TransactionTypeDesc.Contains(request.SearchValue)
                || x.AccountNumber.Contains(request.SearchValue))
                    .ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving  Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
