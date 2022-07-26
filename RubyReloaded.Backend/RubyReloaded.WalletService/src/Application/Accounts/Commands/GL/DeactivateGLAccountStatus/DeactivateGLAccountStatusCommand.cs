using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
    public class DeactivateGLAccountStatusCommand : IRequest<Result>
    {
        public int GLAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class DeactivateGLAccountStatusHandler : IRequestHandler<DeactivateGLAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeactivateGLAccountStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeactivateGLAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var GLAccount = await _context.Accounts.FindAsync(request.GLAccountId);
                if (GLAccount == null)
                {
                    return Result.Failure(new string[] { "Invalid GLAccount" });
                }
                GLAccount.AccountStatus = AccountStatus.Deactivated;
                GLAccount.AccountStatusDesc = AccountStatus.Deactivated.ToString();

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("GL Account Deactivation was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "GLAccount Deactivation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
