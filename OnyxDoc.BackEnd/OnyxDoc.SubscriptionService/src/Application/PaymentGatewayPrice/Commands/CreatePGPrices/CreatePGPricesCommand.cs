using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGPrices.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class CreatePGPricesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CreatePGPriceRequest> PGPrices { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePGPricesCommandHandler : IRequestHandler<CreatePGPricesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGPricesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePGPricesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PGPrice>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PGPrices)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PGPrices.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanPricingId == item.SubscriptionPlanPricingId && a.PaymentGatewayPriceCode == item.PaymentGatewayPriceCode && a.PaymentGateway == item.PaymentGateway);

                    if (exists)
                    {
                        return Result.Failure($"Price already exists for '{item.PaymentGateway}'.");
                    }
                    var entity = new PGPrice
                    {

                        Name = item.SubscriptionPlanPricingId + "_" + item.PaymentGateway,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanPricingId = item.SubscriptionPlanPricingId,
                        PaymentGatewayPriceId = item.PaymentGatewayPriceId,
                        PaymentGatewayPriceCode = item.PaymentGatewayPriceCode,
                        PaymentGateway = item.PaymentGateway,
                        PaymentGatewayDesc = item.PaymentGateway.ToString(),

                        UserId = request.UserId,
                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.PGPrices.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGPriceDto>>(list);
                return Result.Success("Payment gateway price created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway price creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePGPriceRequest item)
        {
            CreatePGPriceRequestValidator validator = new CreatePGPriceRequestValidator();

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
