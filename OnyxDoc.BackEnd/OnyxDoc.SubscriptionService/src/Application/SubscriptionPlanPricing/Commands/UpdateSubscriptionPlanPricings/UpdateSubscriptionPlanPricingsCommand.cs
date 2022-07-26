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
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class UpdateSubscriptionPlanPricingsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPlanPricingsCommandHandler : IRequestHandler<UpdateSubscriptionPlanPricingsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateSubscriptionPlanPricingsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateSubscriptionPlanPricingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<SubscriptionPlanPricing>();
                await _context.BeginTransactionAsync();

                var subscriptionPlan = await _context.SubscriptionPlans
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.SubscriptionPlanId);

                foreach (var item in request.SubscriptionPlanPricings)
                {
                    this.ValidateItem(item);

                    //check if the name of the subscription type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.SubscriptionPlanPricings
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && x.CurrencyId == item.CurrencyId);

                    if (UpdatedEntityExists)
                    {
                        continue;
                        //return Result.Failure($"The currency code '{item.CurrencyCode}' is already configured for this subscription.");
                    }

                    var entity = await _context.SubscriptionPlanPricings
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && (x.Id == item.Id || x.CurrencyId == item.CurrencyId || x.CurrencyCode == item.CurrencyCode))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new SubscriptionPlanPricing
                        {
                            Name = item.CurrencyCode,
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionPlanId = request.SubscriptionPlanId,
                            PricingPlanType = item.PricingPlanType,
                            PricingPlanTypeDesc = item.PricingPlanType.ToString(),

                            Amount = item.Amount,
                            CurrencyId = item.CurrencyId,
                            CurrencyCode = item.CurrencyCode,
                            Discount = item.Discount,

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
                        entity.CurrencyId = item.CurrencyId;
                        entity.CurrencyCode = item.CurrencyCode;
                        entity.PricingPlanType = item.PricingPlanType;
                        entity.PricingPlanTypeDesc = item.PricingPlanType.ToString();
                        entity.Amount = item.Amount;
                        entity.Discount = entity.SubscriptionPlan.AllowDiscount ? item.Discount : 0;
                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.SubscriptionPlanPricings.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionPlanPricingDto>>(list);
                return Result.Success("Subscription plan pricings update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan pricing update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateSubscriptionPlanPricingRequest item)
        {
            UpdateSubscriptionPlanPricingRequestValidator validator = new UpdateSubscriptionPlanPricingRequestValidator();

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
