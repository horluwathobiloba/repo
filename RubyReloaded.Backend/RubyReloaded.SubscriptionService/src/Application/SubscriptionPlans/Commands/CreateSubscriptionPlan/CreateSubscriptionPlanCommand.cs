using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RubyReloaded.SubscriptionService.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using RubyReloaded.SubscriptionService.Domain.Enums;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlans.Queries;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlans.Commands
{
    public class CreateSubscriptionPlanCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }
        [DefaultValue(1)]
        public int NumberOfUsers { get; set; }

        [DefaultValue(5)]
        public int NumberOfTemplates { get; set; }

        [DefaultValue(10)]
        public int StorageSize { get; set; }

        [DefaultValue(StorageSizeType.GB)]
        public StorageSizeType StorageSizeType { get; set; }
        public string StorageSizeTypeDesc { get; set; }
        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }
        public string Period { get; set; }

        [DefaultValue(false)]
        public bool AllowFreeTrial { get; set; }

        [DefaultValue(false)]
        public bool AllowDiscount { get; set; }

        [DefaultValue(false)]
        public bool ShowSubscribeButton { get; set; }

        [DefaultValue(false)]
        public bool ShowContactUsButton { get; set; }

        [DefaultValue(0)]
        public decimal Discount { get; set; }
        public List<UpdateSubscriptionPlanPricingRequest> SubscriptionPlanPricings { get; set; }
        public List<UpdateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private INotificationService _notificationService;
        private IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public CreateSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator,
            IAuthService authService, IConfiguration configuration, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _authService = authService;
            _configuration = configuration;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                //get user object
                var user = await _authService.GetUserAsync(request.AccessToken, request.SubscriberId, request.UserId, request.UserId);

                if (user == null)
                {
                    return Result.Failure("UserId is not valid");
                }
                var exists = await _context.SubscriptionPlans.AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Name.ToLower().Trim() == request.Name.ToLower().Trim());

                if (exists)
                {
                    return Result.Failure($"Subscription plan name already exists!");
                }

                var entity = new Domain.Entities.SubscriptionPlan
                {
                    Name = request.Name,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.SubscriberName,
                    AllowDiscount = request.AllowDiscount,
                    AllowFreeTrial = request.AllowFreeTrial,
                    AllowMonthlyPricing = request.AllowMonthlyPricing,
                    AllowYearlyPricing = request.AllowYearlyPricing,
                    Discount = request.AllowDiscount ? request.Discount : 0,
                    NumberOfTemplates = request.NumberOfTemplates,
                    NumberOfUsers = request.NumberOfUsers,
                    Period = request.Period,
                    ShowContactUsButton = request.ShowContactUsButton,
                    ShowSubscribeButton = request.ShowSubscribeButton,
                    StorageSize = request.StorageSize,
                    StorageSizeType = request.StorageSizeType,
                    StorageSizeTypeDesc = request.StorageSizeTypeDesc,
                    SubscriptionType = request.SubscriptionType,
                    SubscriptionTypeDesc = request.StorageSizeTypeDesc,

                    UserId = request.UserId,
                    CreatedByEmail = user.Entity.Email,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.BeginTransactionAsync();
                await _context.SubscriptionPlans.AddAsync(entity);
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
                    var command = new UpdateSubscriptionPlanPricingsCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = entity.Id,
                        SubscriptionPlanPricings = request.SubscriptionPlanPricings,
                        UserId = request.UserId,
                        AccessToken =  request.AccessToken                        
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
                        .ThenBy(a => a.FeatureName).ToList();

                    var command = new UpdateSubscriptionPlanFeaturesCommand
                    {
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = entity.Id,
                        SubscriptionPlanFeatures = features,
                        UserId = request.UserId,
                        AccessToken = request.AccessToken
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

                //if (emailList != null && emailList.Count > 0)
                //{
                //    await _emailService.SendBulkEmail(emailList);
                //}

                var result = _mapper.Map<SubscriptionPlanDto>(entity);
                return Result.Success("Subscription plan request created successfully!", result);

            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure($"Subscription plan request creation failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }
}
