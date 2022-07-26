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
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;
using FluentValidation.Results;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class UpdatePGSubscriptionsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; } 
        public int SubscriptionId { get; set; }
        public List<UpdatePGSubscriptionRequest> PGSubscriptions { get; set; }
        public string UserId { get; set; }
    }

    public class UpdatePGSubscriptionsCommandHandler : IRequestHandler<UpdatePGSubscriptionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdatePGSubscriptionsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdatePGSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<PGSubscription>();
                await _context.BeginTransactionAsync();

                var subscriptionPlan = await _context.SubscriptionPlans
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.Id == request.SubscriptionId);

                foreach (var item in request.PGSubscriptions)
                {
                    this.ValidateItem(item);
 
                    var entity = await _context.PGSubscriptions
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionId == request.SubscriptionId
                        && (x.Id == item.Id || (x.PaymentGatewaySubscriptionCode == item.PaymentGatewaySubscriptionCode && x.PaymentGateway == item.PaymentGateway)))
                        .FirstOrDefaultAsync();

                    if (entity == null || item.Id <= 0)
                    {
                        entity = new PGSubscription
                        {
                            Name = item.PaymentGateway+"-"+item.PaymentGateway.ToString(),
                            SubscriberName = _authService.Subscriber?.Name,
                            PaymentGateway = item.PaymentGateway,
                            PaymentGatewayDesc = item.PaymentGateway.ToString(), 
                            PaymentGatewaySubscriptionCode = item.PaymentGatewaySubscriptionCode, 

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
                        entity.SubscriberName = _authService.Subscriber?.Name;
                        entity.SubscriptionId = item.SubscriptionId;
                        entity.PaymentGatewaySubscriptionCode = item.PaymentGatewaySubscriptionCode; 
                        entity.PaymentGateway = item.PaymentGateway;
                        entity.PaymentGatewayDesc = item.PaymentGateway.ToString();  

                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();

                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                    }
                    list.Add(entity);
                }

                _context.PGSubscriptions.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGSubscriptionDto>>(list);
                return Result.Success("Payment gateway subscription update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway subscription update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdatePGSubscriptionRequest item)
        {
            UpdatePGSubscriptionRequestValidator validator = new UpdatePGSubscriptionRequestValidator();

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
