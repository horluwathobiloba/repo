using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using FluentValidation.Results;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class UpdateSubscriptionPlanFeaturesCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public List<UpdateSubscriptionPlanFeatureRequest> SubscriptionPlanFeatures { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPlanFeaturesCommandHandler : IRequestHandler<UpdateSubscriptionPlanFeaturesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;


        public UpdateSubscriptionPlanFeaturesCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateSubscriptionPlanFeaturesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);
                var list = new List<SubscriptionPlanFeature>();
                await _context.BeginTransactionAsync();

                foreach (var item in request.SubscriptionPlanFeatures)
                {
                    this.ValidateItem(item);

                    //check if the name of the vendor type already exists and conflicts with this new name 
                    var UpdatedEntityExists = await _context.SubscriptionPlanFeatures
                           .AnyAsync(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && x.FeatureId == item.FeatureId && x.Status == item.Status);

                    if (UpdatedEntityExists)
                    {
                        return Result.Failure($"The feature named '{item.FeatureName}' is already configured for this subscription.");
                    }

                    var entity = await _context.SubscriptionPlanFeatures
                        .Where(x => x.SubscriberId == request.SubscriberId && x.SubscriptionPlanId == request.SubscriptionPlanId && (x.Id == item.Id || x.FeatureId == item.FeatureId))
                        .FirstOrDefaultAsync();

                    if (entity == null)
                    {
                        entity = new SubscriptionPlanFeature
                        {
                            Name = item.FeatureName,
                            SubscriberId = request.SubscriberId,
                            SubscriberName = _authService.Subscriber?.Name,
                            SubscriptionPlanId = request.SubscriptionPlanId,
                            FeatureId = item.FeatureId,
                            FeatureName = item.FeatureName,
                            ParentFeatureId = item.ParentFeatureId,
                            ParentFeatureName = item.ParentFeatureName,
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
                        entity.FeatureId = item.FeatureId;
                        entity.FeatureName = item.FeatureName;
                        entity.ParentFeatureId = item.ParentFeatureId;
                        entity.ParentFeatureName = item.ParentFeatureName;
                        entity.LastModifiedBy = request.UserId;
                        entity.LastModifiedDate = DateTime.Now;
                        entity.Status = item.Status;
                        entity.StatusDesc = item.Status.ToString();
                    }
                    list.Add(entity);
                }

                _context.SubscriptionPlanFeatures.UpdateRange(list);
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                var result = _mapper.Map<List<SubscriptionPlanFeatureDto>>(list);
                return Result.Success("Subscription plan features update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan feature update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

        private void ValidateItem(UpdateSubscriptionPlanFeatureRequest item)
        {
            UpdateSubscriptionFeatureRequestValidator validator = new UpdateSubscriptionFeatureRequestValidator();

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
