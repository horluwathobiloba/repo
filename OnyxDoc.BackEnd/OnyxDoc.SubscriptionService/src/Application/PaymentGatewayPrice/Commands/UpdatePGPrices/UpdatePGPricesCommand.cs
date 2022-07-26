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
using OnyxDoc.SubscriptionService.Application.PGPrices.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class UpdatePGPricesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<UpdatePGPriceRequest> PGPrices { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGPricesCommandHandler : IRequestHandler<UpdatePGPricesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePGPricesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGPricesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PGPrice>();
                await _context.BeginTransactionAsync();

            

                foreach (var item in request.PGPrices)
                {
                    this.ValidateItem(item);
                
                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.PGPrices
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanPricingId == item.SubscriptionPlanPricingId && x.PaymentGatewayPriceId == item.PaymentGatewayPriceId
                           && x.PaymentGatewayPriceId ==  item.PaymentGatewayPriceId);

                    if (UpdatedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"The currency code '{item.CurrencyCode}' is already configured for this subscription.");
                    }

                    var entity = await _context.PGPrices
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanPricingId == item.SubscriptionPlanPricingId 
                        && (x.Id == item.Id || (x.PaymentGatewayPriceCode == item.PaymentGatewayPriceCode && x.PaymentGateway == item.PaymentGateway)))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PGPrice
                        {
                            Name = item.PaymentGateway+"-"+item.PaymentGateway.ToString(),
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionPlanPricingId = item.SubscriptionPlanPricingId,
                            PaymentGateway = item.PaymentGateway,
                            PaymentGatewayDesc = item.PaymentGateway.ToString(), 
                            PaymentGatewayPriceId = item.PaymentGatewayPriceId,
                            PaymentGatewayPriceCode = item.PaymentGatewayPriceCode,

                            UserId = request.UserId,
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
                        entity.SubscriptionPlanPricingId = item.SubscriptionPlanPricingId;
                        entity.PaymentGatewayPriceId = item.PaymentGatewayPriceId;
                        entity.PaymentGatewayPriceCode = item.PaymentGatewayPriceCode;
                        entity.PaymentGateway = item.PaymentGateway;
                        entity.PaymentGatewayDesc = item.PaymentGateway.ToString();  

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PGPrices.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGPriceDto>>(list);
                return Result.Success("Payment gateway price update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway price update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePGPriceRequest item)
        {
            UpdatePGPriceRequestValidator validator = new UpdatePGPriceRequestValidator();

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
