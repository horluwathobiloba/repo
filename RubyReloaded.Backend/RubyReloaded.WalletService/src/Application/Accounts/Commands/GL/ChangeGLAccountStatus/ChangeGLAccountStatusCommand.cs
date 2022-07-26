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
    public class ChangeGLAccountStatusCommand : IRequest<Result>
    {
        public int GLAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeGLAccountStatusHandler : IRequestHandler<ChangeGLAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public ChangeGLAccountStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeGLAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.Accounts.FindAsync(request.GLAccountId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid GLAccount" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "GLAccount deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "GLAccount activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "GLAccount activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "GLAccount status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
