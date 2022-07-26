using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.VirtualAccount.Commands.ChangeVirtualAccount
{
    public class ChangeVirtualAccountStatusCommand : IRequest<Result>
    {
        public int VirtualAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeVirtualAccountStatusCommandHandler : IRequestHandler<ChangeVirtualAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public ChangeVirtualAccountStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeVirtualAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.VirtualAccounts.FindAsync(request.VirtualAccountId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Virtual Account Id not found" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Virtual Account deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Virtual Account activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Virtual Account  activation was successful";
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
                return Result.Failure(new string[] { "Vitual Account status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
