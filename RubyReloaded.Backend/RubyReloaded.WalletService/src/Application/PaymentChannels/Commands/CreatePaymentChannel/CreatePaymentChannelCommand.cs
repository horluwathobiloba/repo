

using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Commands.CreatePaymentChannel
{
    public class CreatePaymentChannelCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal PaymentGatewayFee { get; set; }
        public FeeType PaymentGatewayFeeType { get; set; }
        public int CurrencyId { get; set; }
        public string SettlementAccountNumber { get; set; }
        public Domain.Entities.Bank Bank { get; set; }
        public PaymentChannelType PaymentChannelType { get; set; }
        public PaymentGatewayCategory PaymentGatewayCategory { get; set; }
        public string UserId { get; set; }
        public ICollection<BankServiceRequest> BankServices { get; set; }
        public ICollection<PaymentGatewayServiceRequest> PaymentGatewayServices { get; set; }
    }

    public class CreatePaymentChannelCommandHandler : IRequestHandler<CreatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBase64ToFileConverter _base64ToFileConverter;

        public CreatePaymentChannelCommandHandler(IApplicationDbContext context, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _base64ToFileConverter = base64ToFileConverter;
        }

        public async Task<Result> Handle(CreatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //check if any PaymentChannel for the customer is active or if the name already exists.. If yes, then return a failure response else go ahead and create the PaymentChannel
                var exists = await _context.PaymentChannels.AnyAsync(a => (a.Name.ToUpper() == request.Name.ToUpper() && a.CurrencyId == request.CurrencyId)
                && a.Status == Status.Active);

                if (exists)
                {
                    return Result.Failure(new string[] { "Create new Payment Channel failed because a payment channel name already exists. Please enter a new payment channel name to continue." });
                }
                var currency = await _context.Currencies.FirstOrDefaultAsync(a => a.Id == request.CurrencyId && a.Status == Status.Active);
                if (currency == null)
                {
                    return Result.Failure("Invalid Currency Details");
                }
                await _context.BeginTransactionAsync();

                var entity = new PaymentChannel
                {
                    PaymentGatewayFee = request.PaymentGatewayFee,
                    PaymentGatewayFeeType = request.PaymentGatewayFeeType,
                    PaymentChannelType = request.PaymentChannelType,
                    PaymentChannelTypeDesc = request.PaymentChannelType.ToString(),
                    PaymentGatewayCategory = request.PaymentGatewayCategory,
                    PaymentGatewayCategoryDesc = request.PaymentGatewayCategory.ToString(),
                    Name = request.Name,
                    StatusDesc = Status.Active.ToString(),
                    CurrencyId = request.CurrencyId,
                    CurrencyCode = currency.CurrencyCode.ToString(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.UserId,
                    Status = Status.Active,
                    SettlementAccountNumber=request.SettlementAccountNumber,
                    BankId=request.Bank.Id,
                    Image= await _base64ToFileConverter.ConvertBase64StringToFile(request.Image, request.Name+"_"+ DateTime.Now.Ticks + ".png"),
                };
               await _context.PaymentChannels.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                //Payment Channel Services
                if (request.PaymentGatewayServices!=null)
                {

                    List<PaymentGatewayService> paymentGatewayServices = new List<PaymentGatewayService>();
                    foreach (var paymentGatewayService in request.PaymentGatewayServices)
                    {
                        paymentGatewayServices.Add(new PaymentGatewayService
                        {
                            PaymentChannel=entity,
                            PaymentChannelId=entity.Id,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            Status = paymentGatewayService.Status,
                            StatusDesc = paymentGatewayService.Status.ToString(),
                            PaymentGatewayServiceCategory = paymentGatewayService.PaymentGatewayServiceCategory,
                            Name = paymentGatewayService.PaymentGatewayServiceCategory.ToString(),

                        });
                    }
                   // entity.PaymentGatewayServices = paymentGatewayServices;
                    await _context.PaymentGatewayServices.AddRangeAsync(paymentGatewayServices);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Payment Channel created successfully!", entity);
                }
                //Bank Channel Service
                if (request.BankServices!=null)
                {
                    List<BankService> bankServices = new List<BankService>();
                    foreach (var bankService in request.BankServices)
                    {
                        bankServices.Add(new BankService
                        {
                            PaymentChannel=entity,
                            PaymentChannelId=entity.Id,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            BankServiceCategory = bankService.BankServiceCategory,
                            CommissionFee = bankService.CommissionFee,
                            CommissionFeeType = bankService.CommissionFeeType,
                            MaximumTransactionLimit = bankService.MaximumTransactionLimit,
                            MinimumTransactionLimit = bankService.MinimumTransactionLimit,
                            Status = bankService.Status,
                            StatusDesc = bankService.Status.ToString(),
                            TransactionFee = bankService.TransactionFee,
                            TransactionFeeType = bankService.TransactionFeeType,
                        });
                    }
                    await _context.BankServices.AddRangeAsync(bankServices);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success("Payment Channel created successfully!", entity);
                    //entity.BankServices = bankServices; 
                }
                //there should be a double entry here

               
                await _context.CommitTransactionAsync();
                return Result.Success("Payment Channel created successfully!", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Payment Channel creation failed!", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
