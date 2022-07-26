using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Commands
{
    public class DeleteSubscriptionPlanFeatureCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteSubscriptionFeatureCommandHandler : IRequestHandler<DeleteSubscriptionPlanFeatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteSubscriptionFeatureCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteSubscriptionPlanFeatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.SubscriptionPlanFeatures.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionPlanId ==  request.SubscriptionPlanId && x.Id == request.Id);
                _context.SubscriptionPlanFeatures.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Subscription plan feature deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete Subscription plan feature failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
