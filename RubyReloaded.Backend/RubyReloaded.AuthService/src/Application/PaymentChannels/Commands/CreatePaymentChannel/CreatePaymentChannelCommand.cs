using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.PaymentChannels.Commands.CreatePaymentChannel
{
    public class CreatePaymentChannelCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public decimal TransactionFee { get; set; }
        public TransactionFeeType TransactionFeeType { get; set; }
        public int CurrencyConfigurationId { get; set; }
        public string SettlementAccountNumber { get; set; }
        public string Bank { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class CreatePaymentChannelCommandHandler : IRequestHandler<CreatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentChannelCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any PaymentChannel for the customer is active or if the name already exists.. If yes, then return a failure response else go ahead and create the PaymentChannel
                var exists = await _context.PaymentChannels.AnyAsync(a => (a.Name.ToUpper() == request.Name.ToUpper() && a.CurrencyConfigurationId == request.CurrencyConfigurationId)
                && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { "Create new Payment Channel failed because a payment channel name already exists. Please enter a new payment channel name to continue." });
                }

                var entity = new PaymentChannel
                {
                    Name = request.Name,
                    StatusDesc = Status.Active.ToString(),
                    CurrencyConfigurationId=request.CurrencyConfigurationId,
                    TransactionFeeType=request.TransactionFeeType,
                    TransactionFee=request.TransactionFee,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    SettlementAccountNumber=request.SettlementAccountNumber,
                    Bank=request.Bank
                };
                _context.PaymentChannels.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Payment Channel created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment Channel creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
