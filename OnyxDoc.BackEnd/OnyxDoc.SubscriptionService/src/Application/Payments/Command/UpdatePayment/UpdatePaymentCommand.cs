using OnyxDoc.SubscriptionService.Application.Common.Exceptions;
using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using OnyxDoc.SubscriptionService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.SubscriptionService.Application.Common.Models;
using System;
using Microsoft.EntityFrameworkCore;
using OnyxDoc.SubscriptionService.Domain.Enums;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest<Result>
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }  
        public decimal FeeRate { get; set; }
        public decimal TransactionFee { get; set; }
        public ApplicationType LoggedInDevice { get; set; }
    }

    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;

        public UpdatePaymentCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Payments.FirstOrDefaultAsync(a=> a.SessionId == request.SessionId);

            try
            {
                if (entity == null)
                {
                    return Result.Failure("Invalid Payment specified.");
                }

                entity.Description = request.Description; 
                entity.FeeRate = request.FeeRate;
                entity.TransactionFee = request.TransactionFee;  
                entity.LastModifiedBy = request.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _context.Payments.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success($"Payment \"{entity.SessionId}\" updated successfully");

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { ex?.Message ?? ex?.InnerException?.Message
    });
            }

        }
    }
}
