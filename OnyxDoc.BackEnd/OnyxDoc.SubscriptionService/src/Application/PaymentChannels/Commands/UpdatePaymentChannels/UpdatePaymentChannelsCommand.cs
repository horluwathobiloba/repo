using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands
{
    public class UpdatePaymentChannelsCommand : AuthToken, IRequest<Result>
    {
        public List<UpdatePaymentChannelRequest> PaymentChannels { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePaymentChannelsCommandHandler : IRequestHandler<UpdatePaymentChannelsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePaymentChannelsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePaymentChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PaymentChannel>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.PaymentChannels)
                {

                    //check if the name of the vendor type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.PaymentChannels
                        .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id != item.Id && x.Name == item.Name && x.CurrencyCode == item.CurrencyCode);

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"Another currency named {item.Name.ToString()} already exists for this payment channel. Please change the name and try again.");
                    }
                    var entity = await _context.PaymentChannels.Where(x => x.SubscriberId == request.SubscriberId && x.Id == item.Id)
                                           .FirstOrDefaultAsync();

                    var currency = await _context.PaymentChannels.FirstOrDefaultAsync(x => x.CurrencyCode == item.CurrencyCode);

                    if (currency == null || currency.Id <= 0)
                    {
                        return Result.Failure("Invalid currency specified");
                    }

                    if (entity == null)
                    {
                        entity = new PaymentChannel
                        {
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            Name = item.Name,
                            CurrencyId = currency.Id,
                            CurrencyCode = item.CurrencyCode,
                            CurrencyCodeDesc = item.CurrencyCode.ToString(),
                            TransactionRateType = item.TransactionRateType,
                            TransactionRateTypeDesc = item.TransactionRateType.ToString(),
                            TransactionFee = item.TransactionFee,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            LastModifiedBy = request.UserId,
                            LastModifiedDate = DateTime.Now,
                            Status = Status.Active,
                            StatusDesc = Status.Active.ToString()
                        };
                    }
                    else
                    {
                        entity.Name = item.Name;
                        entity.CurrencyId = currency.Id;
                        entity.CurrencyCode = item.CurrencyCode;
                        entity.CurrencyCodeDesc = item.CurrencyCode.ToString();
                        entity.TransactionRateType = item.TransactionRateType;
                        entity.TransactionRateTypeDesc = item.TransactionRateType.ToString();
                        entity.TransactionFee = item.TransactionFee;

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PaymentChannels.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PaymentChannelDto>>(list);
                return Result.Success("PaymentChannels update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"PaymentChannel update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePaymentChannelRequest item)
        {
            UpdatePaymentChannelRequestValidator validator = new UpdatePaymentChannelRequestValidator();

            ValidationResult validateResult = validator.Validate(item);
            string validateError = null;

            if (!validateResult.IsValid)
            {
                foreach (var failure in validateResult.Errors)
                {
                    validateError += "Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage + "\n";
                }
                throw new Exception(validateError);
            }
        }

    }
}
