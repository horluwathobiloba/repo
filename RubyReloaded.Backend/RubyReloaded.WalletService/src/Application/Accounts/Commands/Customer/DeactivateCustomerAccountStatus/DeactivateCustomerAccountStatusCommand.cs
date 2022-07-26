using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Accounts.Commands.DeactivateCustomerAccountStatus
{
    public class DeactivateCustomerAccountStatusCommand : IRequest<Result>
    {
        public int CustomerAccountId { get; set; }
        public string UserId { get; set; }
    }

    public class DeactivateCustomerAccountStatusHandler : IRequestHandler<DeactivateCustomerAccountStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeactivateCustomerAccountStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeactivateCustomerAccountStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _context.BeginTransactionAsync();
                var CustomerAccount = await _context.Accounts.FindAsync(request.CustomerAccountId);
                if (CustomerAccount == null)
                {
                    return Result.Failure(new string[] { "Invalid Customer Account" });
                }
                CustomerAccount.AccountStatus = AccountStatus.Deactivated;
                CustomerAccount.AccountStatusDesc = AccountStatus.Deactivated.ToString();

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();
                return Result.Success("Customer Account Deactivation was successful");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "CustomerAccount Deactivation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
