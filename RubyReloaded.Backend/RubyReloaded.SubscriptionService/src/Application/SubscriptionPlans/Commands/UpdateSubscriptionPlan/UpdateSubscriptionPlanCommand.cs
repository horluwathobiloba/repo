using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlans.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlans.Commands
{
    public class UpdateSubscriptionPlanCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfTemplates { get; set; }
        public int StorageSize { get; set; }
        public StorageSizeType StorageSizeType { get; set; }
        public string StorageSizeTypeDesc { get; set; }
        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }
        public string Period { get; set; }
        public bool AllowFreeTrial { get; set; }
        public bool AllowDiscount { get; set; }
        public bool ShowSubscribeButton { get; set; }
        public bool ShowContactUsButton { get; set; }
        public decimal Discount { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public List<UpdateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.SubscriptionPlans.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id);
                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription plan specified.");
                }

                var subscriptionExists = await _context.SubscriptionPlans.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.Id && x.Name == request.Name);

                if (subscriptionExists)
                {
                    return Result.Failure($"Another subscription plan named {request.Name.ToString()} already exists.");
                }

                entity.Name = request.Name.ToString();
                entity.AllowDiscount = request.AllowDiscount;
                entity.AllowFreeTrial = request.AllowFreeTrial;
                entity.AllowMonthlyPricing = request.AllowMonthlyPricing;
                entity.AllowYearlyPricing = request.AllowYearlyPricing;
                entity.Discount = request.AllowDiscount ? request.Discount : 0;
                entity.NumberOfTemplates = request.NumberOfTemplates;
                entity.NumberOfUsers = request.NumberOfUsers;
                entity.Period = request.Period;
                entity.ShowContactUsButton = request.ShowContactUsButton;
                entity.ShowSubscribeButton = request.ShowSubscribeButton;
                entity.StorageSize = request.StorageSize;
                entity.StorageSizeType = request.StorageSizeType;
                entity.StorageSizeTypeDesc = request.StorageSizeTypeDesc;
                entity.SubscriptionType = request.SubscriptionType;
                entity.SubscriptionTypeDesc = request.StorageSizeTypeDesc;
                entity.Name = request.Name;
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                await _context.BeginTransactionAsync();

                _context.SubscriptionPlans.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var notification = new Notification
                {
                    Message = "",
                    NotificationStatus = NotificationStatus.Unread,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now
                };

                //TO DO: Call the API for notifications
                //await _context.Notifications.AddAsync(notification);
                //await _notificationService.SendNotification(request.Email, notification.Message);

                if (request.SubscriptionPlanPricings != null && request.SubscriptionPlanPricings.Count > 0)
                {
                    //save the pricings
                    var pricings = request.SubscriptionPlanPricings
                        .OrderBy(a => a.CurrencyCode)
                        .ThenBy(a => a.IsDeleted).ToList();

                    var command = new UpdateSubscriptionPlanPricingsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.Id,
                        SubscriptionPlanPricings = request.SubscriptionPlanPricings,
                        UserId = request.UserId    
                    };

                    var handler = new UpdateSubscriptionPlanPricingsCommandHandler(_context, _mapper, _authService);
                    var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    //  var recipientsResult = _mediator.Send(command).Result;
                    if (subscriptionFeatureResult.Succeeded == false)
                    {
                        throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    }
                }

                if (request.SubscriptionPlanFeatures != null && request.SubscriptionPlanFeatures.Count > 0)
                {
                    //save the features
                    var features = request.SubscriptionPlanFeatures
                        .OrderBy(a => a.ParentFeatureId)
                        .ThenBy(a => a.ParentFeatureId).ToList();

                    var command = new UpdateSubscriptionPlanFeaturesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.Id,
                        SubscriptionPlanFeatures = request.SubscriptionPlanFeatures,
                        UserId = request.UserId
                    };

                    var handler = new UpdateSubscriptionPlanFeaturesCommandHandler(_context, _mapper, _authService);
                    var subscriptionFeatureResult = await handler.Handle(command, cancellationToken);
                    //  var recipientsResult = _mediator.Send(command).Result;
                    if (subscriptionFeatureResult.Succeeded == false)
                    {
                        throw new Exception(subscriptionFeatureResult.Error + subscriptionFeatureResult.Message);
                    }
                }

                await _context.CommitTransactionAsync();

                var result = _mapper.Map<SubscriptionPlanDto>(entity);
                return Result.Success("Payment channel update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment channel update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }


}
