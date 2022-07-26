using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.ActivateCustomerAccountStatus
{
    public class ActivateCustomerAccountStatusCommand : IRequest<Result>
    {
        public int CustomerAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class ActivateCustomerAccountStatusCommandHandler : IRequestHandler<ActivateCustomerAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public ActivateCustomerAccountStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ActivateCustomerAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var entity = await _context.Accounts.FindAsync(request.CustomerAccountId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid CustomerAccount" });
                }
                entity.AccountStatus = Domain.Enums.AccountStatus.Active;
                entity.AccountStatusDesc = Domain.Enums.AccountStatus.Active.ToString();

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Customer Account activation  successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Customer Account activation not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
