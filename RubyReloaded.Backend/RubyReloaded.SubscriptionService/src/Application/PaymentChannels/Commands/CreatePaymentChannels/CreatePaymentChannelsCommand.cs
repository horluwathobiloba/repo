using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.PaymentChannels.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.PaymentChannels.Commands
{
    public class CreatePaymentChannelsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public List<CreatePaymentChannelRequest> PaymentChannels { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePaymentChannelsCommandHandler : IRequestHandler<CreatePaymentChannelsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePaymentChannelsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePaymentChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PaymentChannel>();

                await _context.BeginTransactionAsync();
               
                foreach (var item in request.PaymentChannels)
                {
                   this.ValidateItem(item);
                   
                    var exists = await _context.PaymentChannels.AnyAsync(x => x.SubscriberId == request.SubscriberId && (x.CurrencyCode == item.CurrencyCode || x.Name == item.Name));
                    if (exists)
                    {
                        return Result.Failure($"Currency code '{item.CurrencyCode}' or payment channel name '{item.Name}' already exists.");
                    }

                    var entity = new PaymentChannel
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        Name = item.Name,
                        CurrencyId = item.CurrencyId,
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
                    list.Add(entity);
                }
                await _context.PaymentChannels.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PaymentChannelDto>>(list);
                return Result.Success("PaymentChannels created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"PaymentChannels creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePaymentChannelRequest item)
        {
            CreatePaymentChannelRequestValidator validator = new CreatePaymentChannelRequestValidator();

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
