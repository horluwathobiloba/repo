using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.WithdrawalSettings.Commands
{
    public class AddWithdrawalSettingCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string UserId { get; set; }
    }
    public class AddWithdrawalSettingsCommandHandler : IRequestHandler<AddWithdrawalSettingCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddWithdrawalSettingsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(AddWithdrawalSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any PaymentChannel for the customer is active or if the name already exists.. If yes, then return a failure response else go ahead and create the PaymentChannel
                var exists = await _context.WithdrawalSettings.AnyAsync(a => a.UserId == request.UserId);
                if (exists)
                {
                    return Result.Failure("Withdrawal Settings for this user already exist" );
                }

                var entity = new WithdrawalSetting
                {
                    AccountNumber = request.AccountNumber,
                    BankName = request.BankName,
                    CreatedDate = DateTime.Now,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserId = request.UserId
                };
                await _context.WithdrawalSettings.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Beneficiary created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure( "Withdrwal settings creation failed!"+ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
