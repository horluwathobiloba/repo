using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using RubyReloaded.SubscriptionService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanPricings.Commands
{
    public class DeleteSubscriptionPlanPricingCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeleteSubscriptionPlanPricingCommandHandler : IRequestHandler<DeleteSubscriptionPlanPricingCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeleteSubscriptionPlanPricingCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeleteSubscriptionPlanPricingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.SubscriptionPlanPricings.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionPlanId == request.SubscriptionPlanId && x.Id == request.Id);

                _context.SubscriptionPlanPricings.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Subscription pricing deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete subscription pricing failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
