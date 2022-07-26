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
    public class GetFrequentlyTransferredQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }
    public class GetFrequentlyTransferredQueryHandler : IRequestHandler<GetFrequentlyTransferredQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetFrequentlyTransferredQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetFrequentlyTransferredQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var transactions = _context.WalletTransactions
                    .Where(x=>x.UserId==request.UserId)
                    .GroupBy(x => x.ReciepientName)
                    .Where(g=>g.Count() > 5)
                    .Select(x =>x.Key).ToList();
                var beneficiaries = await _context.WalletBeneficiaries
                    .Where(x => x.UserId == request.UserId)
                    .Select(x => x.Username)
                    .ToListAsync();
                var freqeuentlyTransferred = beneficiaries.Except(transactions);
                return Result.Success(freqeuentlyTransferred);


            }
            catch (Exception ex)
            {
                return Result.Failure("Retreiving was not successful" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }

}
