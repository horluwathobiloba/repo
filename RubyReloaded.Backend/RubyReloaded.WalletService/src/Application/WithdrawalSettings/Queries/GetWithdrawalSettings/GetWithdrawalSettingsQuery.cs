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
   public class GetWithdrawalSettingsQuery : IRequest<Result>
   {
        public string UserId { get; set; }
   }
    public class GetWithdrawalSettingsQueryHandler : IRequestHandler<GetWithdrawalSettingsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        public GetWithdrawalSettingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Result> Handle(GetWithdrawalSettingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var wallets = await _context.WithdrawalSettings.FirstOrDefaultAsync(a => a.UserId == request.UserId);
                if (wallets == null)
                {
                    return Result.Failure("Invalid credentials");
                }
                return Result.Success(wallets);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving details was not successful", ex?.Message ??ex?.InnerException.Message });
            }
        }
    }
}
