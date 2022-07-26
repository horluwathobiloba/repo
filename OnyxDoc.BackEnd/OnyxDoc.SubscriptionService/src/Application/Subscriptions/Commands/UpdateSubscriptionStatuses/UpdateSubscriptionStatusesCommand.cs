using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.Subscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Subscriptions.Commands
{
    public class UpdateSubscriptionStatusesCommand : AuthToken, IRequest<Result>
    {
        public int SuperAdminSubscriberId { get; set; }
        public List<SubscriberDto> Subscribers { get; set; }
        public List<UpdateSubscriptionStatusesRequest> Subscriptions { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionStatusesCommandHandler : IRequestHandler<UpdateSubscriptionStatusesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ISubscriberService _subscriberService;

        public UpdateSubscriptionStatusesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService, ISubscriberService subscriberService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
            _subscriberService = subscriberService;
        }

        public async Task<Result> Handle(UpdateSubscriptionStatusesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = _authService.ValidateSubscriberData(request.AccessToken, request.SuperAdminSubscriberId, request.UserId).Result;

                if (_authService?.Subscriber?.SubscriberAccessLevel != SubscriberAccessLevel.System)
                {
                    return Result.Failure($"Your subscriber access is denied for this operation. Please contact the system administrator.");
                }

                var user = _authService?.User;
                if (user?.Role?.RoleAccessLevel != RoleAccessLevel.SuperAdmin || user?.Role?.RoleAccessLevel != RoleAccessLevel.ServiceUser)
                {
                    return Result.Failure($"You do not have access to perform this operation. Please contact your system administrator.");
                }

                var list = new List<Subscription>();
                string message = "";

                await _context.BeginTransactionAsync();

                foreach (var item in request.Subscriptions)
                {
                    var entity = _context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriberId == item.SubscriberId && x.SubscriptionPlanId == item.SubscriptionPlanId && x.Id == item.Id).Result;

                    if (entity == null)
                    {
                        return Result.Failure("Invalid currency!");
                    }

                    switch (item.SubscriptionStatus)
                    {
                        case SubscriptionStatus.Active:
                            entity.Status = Status.Active;
                            message += $"Subscription for subscriber {entity.SubscriberName} is now active!" + Environment.NewLine;
                            break;
                        case SubscriptionStatus.Cancelled:
                            entity.Status = Status.Deactivated;
                            message += $"Subscription for subscriber {entity.SubscriberName} is now cancelled!" + Environment.NewLine;
                            break;
                        case SubscriptionStatus.Expired:
                            entity.Status = Status.Deactivated;
                            message += $"Subscription for subscriber {entity.SubscriberName} is now expired!" + Environment.NewLine;
                            break;
                        case SubscriptionStatus.FreeTrial:
                            entity.Status = Status.Active;
                            message = $"Subscription for subscriber {entity.SubscriberName} is now in free trial mode!" + Environment.NewLine;
                             await _subscriberService.ActivateSubscriberFreeTrialAsync(request.AccessToken, item.Subscriber.Id, request.UserId);  //send an asynchronous call to subscribers
                            break;
                        case SubscriptionStatus.ProcessingPayment:
                            entity.Status = item.Subscriber.FreeTrialCompleted ? Status.Inactive : entity.Status = Status.Active;
                            message = $"Subscription for subscriber {entity.SubscriberName} is now been processed for payment!" + Environment.NewLine;
                              await _subscriberService.CompleteSubscriberFreeTrialAsync(request.AccessToken, item.Subscriber.Id, request.UserId);  //send an asynchronous call to subscribers
                            break;
                        default:
                            break;
                    }

                    entity.SubscriptionStatus = item.SubscriptionStatus;
                    entity.StatusDesc = entity.Status.ToString();
                    entity.LastModifiedBy = request.UserId;
                    entity.LastModifiedDate = DateTime.Now;

                    list.Add(entity); //add the record to the list
                }

                _context.Subscriptions.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionDto>>(list);
                return Result.Success($"Subscription status update was successful for {list.Count} records!", result);

                //var result = _mapper.Map<List<SubscriptionDto>>(message, result);
                //return Result.Success(message, result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription status update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }
    }
}
