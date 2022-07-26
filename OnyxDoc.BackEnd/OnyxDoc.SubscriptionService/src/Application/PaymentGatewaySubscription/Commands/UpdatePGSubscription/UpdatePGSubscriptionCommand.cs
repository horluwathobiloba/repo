using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGSubscriptions.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGSubscriptions.Commands
{
    public class UpdatePGSubscriptionCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public int SubscriptionId { get; set; }
        public string PaymentGatewaySubscriptionCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class UpdatePGSubscriptionCommandHandler : IRequestHandler<UpdatePGSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdatePGSubscriptionCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePGSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PGSubscriptions.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId
                && x.SubscriptionId == request.SubscriptionId && (x.Id == request.Id || (x.PaymentGatewaySubscriptionCode == request.PaymentGatewaySubscriptionCode && x.PaymentGateway == request.PaymentGateway)));

                if (entity == null)
                {
                    return Result.Failure($"Invalid subscription currency specified.");
                }

                entity.SubscriberName = _authService.Subscriber?.Name;
                entity.PaymentGatewaySubscriptionCode = request.PaymentGatewaySubscriptionCode;
                entity.PaymentGateway = request.PaymentGateway;
                entity.PaymentGatewayDesc = request.PaymentGateway.ToString();

                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PGSubscriptions.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGSubscriptionDto>(entity);
                return Result.Success("Payment gateway subscription update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway subscription update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
