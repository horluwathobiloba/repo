using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WithdrawalSettings.Commands
{
    public class UpdateWithdrawalSettingsCommand:IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string UserId { get; set; }
    }
    public class UpdateWithdrawalSettingsHandler : IRequestHandler<UpdateWithdrawalSettingsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateWithdrawalSettingsHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(UpdateWithdrawalSettingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.WithdrawalSettings.FirstOrDefaultAsync(x => x.UserId == request.UserId);
                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.AccountNumber = request.AccountNumber;
                entity.BankName = request.BankName;
                entity.LastModifiedDate = DateTime.Now;
                _context.WithdrawalSettings.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                //  await _context.CommitTransactionAsync();
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                // _context.RollbackTransaction();
                return Result.Failure( "Withdrawal settings Update was not successful "+ ex?.Message ?? ex?.InnerException.Message);
            }

        }
    }

}
