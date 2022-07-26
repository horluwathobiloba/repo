using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand
{
    public class UpdatePaymentChannelCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string LoggedInUserId { get; set; }
        public string Name { get; set; }
        public decimal TransactionFee { get; set; }
        public TransactionFeeType TransactionFeeType { get; set; }
        public int CurrencyConfigurationId { get; set; }
        public string SettlementAccountNumber { get; set; }
        public string Bank { get; set; }

    }

    public class UpdatePaymentChannelCommandHandler : IRequestHandler<UpdatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdatePaymentChannelCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.PaymentChannels.FindAsync(request.Id);

            try
            {
                if (entity == null)
                {
                    return Result.Failure("Invalid PaymentChannel specified.");
                }

                entity.Name = request.Name;
               
                entity.CurrencyConfigurationId = request.CurrencyConfigurationId;
                entity.TransactionFeeType = request.TransactionFeeType; 
                entity.TransactionFee = request.TransactionFee;
                entity.Bank = request.Bank;
                entity.SettlementAccountNumber = request.SettlementAccountNumber;
                entity.LastModifiedBy = request.LoggedInUserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success($"PaymentChannel \"{entity.Name}\" updated successfully");

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { ex?.Message ?? ex?.InnerException?.Message});
            }

        }
    }
}
