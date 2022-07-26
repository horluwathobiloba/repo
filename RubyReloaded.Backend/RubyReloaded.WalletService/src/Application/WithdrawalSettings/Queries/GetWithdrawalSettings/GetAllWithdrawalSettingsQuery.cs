using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WithdrawalSettings.Queries.GetWithdrawalSettings
{
    public class GetAllWithdrawalSettingsQuery:IRequest<Result>
    {
    }
    public class GetAllWithdrawalSettingsHandler : IRequestHandler<GetAllWithdrawalSettingsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetAllWithdrawalSettingsHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetAllWithdrawalSettingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var details = await _context.WithdrawalSettings.ToListAsync();
                return Result.Success(details);
            }
            catch (Exception ex)
            {
                return Result.Failure("Retrieving details was not successful"+ ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
