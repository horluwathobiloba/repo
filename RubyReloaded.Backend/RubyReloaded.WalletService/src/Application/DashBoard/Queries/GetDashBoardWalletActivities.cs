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
    public class GetDashBoardWalletActivities : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetDashBoardWalletActivitiesHandler : IRequestHandler<GetDashBoardWalletActivities, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetDashBoardWalletActivitiesHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetDashBoardWalletActivities request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WalletTransactions.Where(x => x.CreatedBy == request.UserId).ToListAsync();
                var credit = result.Where(x => x.TransactionType == Domain.Enums.TransactionType.Credit).Select(x => x.TransactionAmount).Sum();
                var debit = result.Where(x => x.TransactionType == Domain.Enums.TransactionType.Debit).Select(x => x.TransactionAmount).Sum();
                return Result.Success(new { credit,debit});
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Wallet Transaction was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
