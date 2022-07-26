using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Application.PGCustomers.Queries;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Commands
{
    public class UpdatePGCustomerCommand : AuthToken, IRequest<Result>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateSubscriptionCurrencyCommandHandler : IRequestHandler<UpdatePGCustomerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateSubscriptionCurrencyCommandHandler(IApplicationDbContext context, IMapper mapper, IAuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> Handle(UpdatePGCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var respo = await _authService.ValidateSubscriberData(request.AccessToken, request.SubscriberId, request.UserId);

                var entity = await _context.PGCustomers.FirstOrDefaultAsync(x => x.SubscriberId == request.SubscriberId 
                && x.PaymentGatewayCustomerCode == request.PaymentGatewayCustomerCode && x.Id == request.Id);

                if (entity == null)
                {
                    return Result.Failure($"Invalid payment gateway customer specified.");
                }

                entity.PaymentGatewayCustomerCode = request.PaymentGatewayCustomerCode;
                entity.PaymentGateway = request.PaymentGateway;

                entity.PaymentGatewayDesc = request.PaymentGateway.ToString();
                entity.UserId = request.UserId;
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;

                _context.PGCustomers.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);

                var result = _mapper.Map<PGCustomerDto>(entity);
                return Result.Success("Payment gateway customer update was successful!", result);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Payment gateway customer update failed. Error: { ex?.Message + Environment.NewLine + ex?.InnerException?.Message}");
            }
        }

    }


}
