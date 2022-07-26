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
    public class GetBankByIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int BankId { get; set; }
    }

    public class GetBankByIdQueryHandler : IRequestHandler<GetBankByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetBankByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetBankByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Banks.FirstOrDefaultAsync(x => x.Id == request.BankId);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving Bank Configuration was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
