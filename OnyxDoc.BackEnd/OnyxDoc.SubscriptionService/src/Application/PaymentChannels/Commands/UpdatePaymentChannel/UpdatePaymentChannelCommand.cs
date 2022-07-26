using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands
{
    public class UpdatePaymentChannelCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public TransactionRateType TransactionRateType { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public decimal TransactionFee { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePaymentChannelCommandHandler : IRequestHandler<UpdatePaymentChannelCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePaymentChannelCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePaymentChannelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PaymentChannels.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid payment channel specified.");
                }

                var currencyexists = await _context.PaymentChannels.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != request.Id
                && x.Name == request.Name && x.CurrencyCode == request.CurrencyCode);

                if (currencyexists)
                {
                    return Result.Failure($"Another currency named {request.Name.ToString()} already exists for this payment channel");
                }

                entity.Name = request.Name;
                entity.CurrencyId = request.CurrencyId;
                entity.CurrencyCode = request.CurrencyCode;
                entity.CurrencyCodeDesc = request.CurrencyCode.ToString();
                entity.TransactionRateType = request.TransactionRateType;
                entity.TransactionRateTypeDesc = request.TransactionRateType.ToString();
                entity.TransactionFee = request.TransactionFee;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PaymentChannels.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PaymentChannelDto>(entity);
                return Result.Success("Payment channel update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment channel update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }


}
