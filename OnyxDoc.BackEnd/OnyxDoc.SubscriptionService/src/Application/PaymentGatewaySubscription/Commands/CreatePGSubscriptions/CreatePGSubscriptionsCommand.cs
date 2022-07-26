using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class CreatePGSubscriptionsCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CreatePGSubscriptionRequest> PGSubscriptions { get; set; }
        public string UserId { get; set; }
    }

    public class CreatePGSubscriptionsCommandHandler : IRequestHandler<CreatePGSubscriptionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreatePGSubscriptionsCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreatePGSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<PGSubscription>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.PGSubscriptions)
                {
                    this.ValidateItem(item);
                    var exists = await _context.PGSubscriptions.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionId == item.SubscriptionId && a.PaymentGatewaySubscriptionCode == item.PaymentGatewaySubscriptionCode && a.PaymentGateway == item.PaymentGateway);

                    if (exists)
                    {
                        return Result.Failure($"Subscription already exists for '{item.PaymentGateway}'.");
                    }
                    var entity = new PGSubscription
                    {

                        Name = item.SubscriptionId + "_" + item.PaymentGateway,
                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionId = item.SubscriptionId,
                        PaymentGatewaySubscriptionId = item.PaymentGatewaySubscriptionId,
                        PaymentGatewaySubscriptionCode = item.PaymentGatewaySubscriptionCode,
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
                await _context.PGSubscriptions.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<PGSubscriptionDto>>(list);
                return Result.Success("Payment gateway subscription created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway subscriptions creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreatePGSubscriptionRequest item)
        {
            CreatePGSubscriptionRequestValidator validator = new CreatePGSubscriptionRequestValidator();

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
