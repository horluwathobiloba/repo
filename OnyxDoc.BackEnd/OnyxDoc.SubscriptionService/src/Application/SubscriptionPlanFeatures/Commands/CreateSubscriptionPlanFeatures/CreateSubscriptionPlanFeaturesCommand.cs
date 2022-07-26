using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PaymentChannels.Commands;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class CreateSubscriptionPlanFeaturesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<CreateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionPlanFeaturesCommandHandler : IRequestHandler<CreateSubscriptionPlanFeaturesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionPlanFeaturesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(CreateSubscriptionPlanFeaturesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var list = new List<SubscriptionPlanFeature>();

                await _context.BeginTransactionAsync();

                foreach (var item in request.SubscriptionPlanFeatures)
                {
                    this.ValidateItem(item);
                    var exists = await _context.SubscriptionPlanFeatures.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && a.FeatureId == item.FeatureId);

                    if (exists)
                    {
                        return Result.Failure($"Subscription plan feature named '{item.FeatureName}' already exists.");
                    }
                    var entity = new SubscriptionPlanFeature
                    {

                        SubscriberId = request.SubscriberId,
                        SubscriberName = _authService.Subscriber?.Name,
                        SubscriptionPlanId = request.SubscriptionPlanId,
                        FeatureId = item.FeatureId,
                        FeatureName = item.FeatureName,
                        ParentFeatureId = item.ParentFeatureId,
                        ParentFeatureName = item.ParentFeatureName,

                        CreatedBy = request.UserId,
                        CreatedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.Now,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    list.Add(entity);
                }
                await _context.SubscriptionPlanFeatures.AddRangeAsync(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionPlanFeatureDto>>(list);
                return Result.Success("Subscription plan features created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan features creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(CreateSubscriptionPlanFeatureRequest item)
        {
            CreateSubscriptionFeatureRequestValidator validator = new CreateSubscriptionFeatureRequestValidator();

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
