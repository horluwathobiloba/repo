
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.PaymentChannels.Commands.UpdatePaymentChannel.UpdatePaymentChannelCommand
{
    public class UpdatePaymentChannelCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal PaymentGatewayFee { get; set; }
        public FeeType PaymentGatewayFeeType { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string CurrencyCode { get; set; }
        public string SettlementAccountNumber { get; set; }
        public Domain.Entities.Bank Bank { get; set; }
        public PaymentChannelType PaymentChannelType { get; set; }
        public PaymentGatewayCategory PaymentGatewayCategory { get; set; }
        public string UserId { get; set; }
        public ICollection<BankServiceRequest> BankServices { get; set; }
        public ICollection<PaymentGatewayServiceRequest> PaymentGatewayServices { get; set; }

    }

    public class UpdatePaymentChannelCommandHandler : IRequestHandler<UpdatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBase64ToFileConverter _base64ToFileConverter;
        public UpdatePaymentChannelCommandHandler(IApplicationDbContext context, IBase64ToFileConverter base64ToFileConverter)
        {
            _context = context;
            _base64ToFileConverter = base64ToFileConverter;
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
                entity.PaymentGatewayFee = request.PaymentGatewayFee;
                entity.PaymentGatewayFeeType = request.PaymentGatewayFeeType;
                entity.PaymentGatewayFeeTypeDesc = request.PaymentGatewayFeeType.ToString();
                entity.PaymentGatewayCategory = request.PaymentGatewayCategory;
                entity.PaymentGatewayCategoryDesc = request.PaymentGatewayCategory.ToString();
                entity.CurrencyId = request.CurrencyId;
                entity.Bank = request.Bank;
                entity.PaymentChannelType = request.PaymentChannelType;
                entity.PaymentChannelTypeDesc = request.PaymentChannelType.ToString();
                entity.SettlementAccountNumber = request.SettlementAccountNumber;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                entity.Image = await _base64ToFileConverter.ConvertBase64StringToFile(request.Image, request.Name + "_" + DateTime.Now.Ticks + ".png");
                //TODO: Update Bank Services

                //TODO: Update Payment Channel Services

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
