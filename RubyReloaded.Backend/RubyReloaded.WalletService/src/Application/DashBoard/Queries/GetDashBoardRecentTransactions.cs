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

namespace RubyReloaded.WalletService.Application.DashBoard.Queries
{
    public class GetDashBoardRecentTransactions:IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetDashBoardRecentTransactionsHandler : IRequestHandler<GetDashBoardRecentTransactions, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetDashBoardRecentTransactionsHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetDashBoardRecentTransactions request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions
                    .Where(x => x.CreatedBy == request.UserId)
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(5)
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
