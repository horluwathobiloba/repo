using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using ReventInject;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class CreateSubscriptionPlanFeatureCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public int ParentFeatureName { get; set; }
        public string UserId { get; set; }
    }

    public class CreateSubscriptionFeatureCommandHandler : IRequestHandler<CreateSubscriptionPlanFeatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateSubscriptionFeatureCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateSubscriptionPlanFeatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var exists = await _context.SubscriptionPlanFeatures.AnyAsync(a => a.SubscriberId == request.SubscriberId
                && a.SubscriptionPlanId == request.SubscriptionPlanId && a.FeatureId == request.FeatureId);

                if (exists)
                {
                    return Result.Failure($"Subscription plan feature named '{request.FeatureName}' already exists.");
                }

                var entity = new SubscriptionPlanFeature
                { 
                    Name =  request.FeatureName,
                    SubscriberId = request.SubscriberId,
                    SubscriberName = _authService.Subscriber?.Name,
                    SubscriptionPlanId = request.SubscriptionPlanId,
                    FeatureId = request.FeatureId,
                    FeatureName = request.FeatureName,
                    ParentFeatureId = request.ParentFeatureId,
                    ParentFeatureName = request.ParentFeatureName,

                    UserId = request.UserId,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = request.UserId,
                    LastModifiedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.SubscriptionPlanFeatures.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<SubscriptionPlanFeatureDto>(entity);
                return Result.Success("Subscription plan feature created successfully!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Subscription plan feature creation failed. Error: { ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}
