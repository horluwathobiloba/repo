using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGPrices.Commands
{
    public class DeletePGPriceCommand : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanPricingId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }


    public class DeletePGPriceCommandHandler : IRequestHandler<DeletePGPriceCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public DeletePGPriceCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Result> Handle(DeletePGPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId ,request.UserId);

                var entity = await _context.PGPrices.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.SubscriptionPlanPricingId == request.SubscriptionPlanPricingId && x.Id == request.Id);

                _context.PGPrices.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Payment gateway price deleted successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete payment gateway price failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }

        }
    }
}
