using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.ChangeCustomerAccountStatus
{
    public class ChangeCustomerAccountStatusCommand : IRequest<Result>
    {
        public int CustomerAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class ChangeCustomerAccountStatusHandler : IRequestHandler<ChangeCustomerAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public ChangeCustomerAccountStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeCustomerAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                await _context.BeginTransactionAsync();
                var entity = await _context.Accounts.FindAsync(request.CustomerAccountId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid Customer Account" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        entity.StatusDesc = Domain.Enums.Status.Inactive.ToString();
                        message = "Customer Account deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Customer Account activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        entity.StatusDesc = Domain.Enums.Status.Active.ToString();
                        message = "Customer Account activation was successful";
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
                return Result.Failure(new string[] { "Customer Account status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
