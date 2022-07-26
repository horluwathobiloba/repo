using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands
{
    public class ActivateGLAccountStatusCommand : IRequest<Result>
    {
        public int GLAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class ActivateGLAccountStatusCommandHandler : IRequestHandler<ActivateGLAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public ActivateGLAccountStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ActivateGLAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var entity = await _context.Accounts.FindAsync(request.GLAccountId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid GLAccount" });
                }
                entity.AccountStatus = Domain.Enums.AccountStatus.Active;
                entity.AccountStatusDesc = Domain.Enums.AccountStatus.Active.ToString();

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("GLAccount activation  successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "GLAccount activation not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
