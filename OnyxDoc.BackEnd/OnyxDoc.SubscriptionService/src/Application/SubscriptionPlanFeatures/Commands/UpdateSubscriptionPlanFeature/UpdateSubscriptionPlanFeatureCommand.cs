using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class UpdateSubscriptionPlanFeatureCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public string ParentFeatureName { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateSubscriptionPlanFeatureCommandHandler : IRequestHandler<UpdateSubscriptionPlanFeatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionPlanFeatureCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdateSubscriptionPlanFeatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var UpdatedEntityExists = await _context.SubscriptionPlanFeatures
                       .AnyAsync(x => x.SubscriberId == request.SubscriberId  && x.SubscriptionPlanId == request.SubscriptionPlanId && x.FeatureId == request.FeatureId);

                if (UpdatedEntityExists)
                {
                    return Result.Failure($"The feature named '{request.FeatureName}' is already configured for this subscription.");
                }

                var entity = await _context.SubscriptionPlanFeatures.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription plan feature specified.");
                }

                entity.FeatureId = request.FeatureId;
                entity.FeatureName = request.FeatureName;
                entity.ParentFeatureId = request.ParentFeatureId;
                entity.ParentFeatureName = request.ParentFeatureName;

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.SubscriptionPlanFeatures.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionPlanFeatureDto>(entity);
                return Result.Success("Subscription plan feature update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan feature update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
