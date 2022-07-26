using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Bank.Queries.GetBank
{
    public class GetBankQuery : IRequest<Result>
    {
        public string UserId { get; set; }
    }

    public class GetBankQueryHandler : IRequestHandler<GetBankQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(GetBankQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Banks.ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Banks was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }

}