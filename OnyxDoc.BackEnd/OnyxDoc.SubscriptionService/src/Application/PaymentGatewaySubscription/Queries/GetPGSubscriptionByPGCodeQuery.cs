using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore; 
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models; 
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries
{
    public class GetPGSubscriptionByPGCodeQuery : AuthToken, IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public string UserId { get; set; }
    }

    public class GetPGSubscriptionByPGCodeQueryHandler : IRequestHandler<GetPGSubscriptionByPGCodeQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public GetPGSubscriptionByPGCodeQueryHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Result> Handle(GetPGSubscriptionByPGCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var PGSubscription = await _context.PGSubscriptions.FirstOrDefaultAsync(a => a.SubscriberId == request.SubscriberId && a.PaymentGatewaySubscriptionCode == request.PaymentGatewaySubscriptionCode);
                if (PGSubscription == null)
                {
                    return Result.Failure("Invalid payment gateway subscription!");
                }

                var result = _mapper.Map<PGSubscriptionDto>(PGSubscription);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occured while retrieving payment gateway subscription. Error: {ex?.Message ?? ex?.InnerException?.Message}");
            }
        }
    }
}