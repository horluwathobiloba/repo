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
using OnyxDoc.SubscriptionService.Domain.Common;

namespace OnyxDoc.SubscriptionService.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentWithStripeResponseCommand : IRequest<Result>
    {
        public string SessionId { get; set; }
        public string PaymentIntentId { get; set; }
        public string StripeStatus { get; set; }
        public string PaymentType { get; set; }
        public DateTime StripeCreatedDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentStatusDesc { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public DateTime FailedDate { get; set; }
        public DateTime CancelledDate { get; set; }
        public DateTime ReversedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public ApplicationType LoggedInDevice { get; set; }
    }

    public class UpdatePaymentResponseCommandHandler : IRequestHandler<UpdatePaymentWithStripeResponseCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdatePaymentResponseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdatePaymentWithStripeResponseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Payments.FirstOrDefaultAsync(a => a.SessionId == request.SessionId);

            try
            {
                if (entity == null)
                {
                    return Result.Failure($"Payment record does not exist for SessionId: {request.SessionId}");
                }

                entity.PaymentIntentId = request.PaymentIntentId;
                entity.ProviderStatus = request.StripeStatus;
                entity.PaymentStatus = request.PaymentStatus;
                entity.PaymentStatusDesc = request.PaymentStatus.ToString();

                entity.CompletedDate = request.CompletedDate;
                entity.FailedDate = request.FailedDate;
                entity.CancelledDate = request.CancelledDate;
                entity.ReversedDate = request.ReversedDate;
                entity.PaymentDate = request.PaymentDate;
                entity.ProviderCreatedDate = request.StripeCreatedDate;

                entity.LastModifiedBy = "SYSTEM";
                entity.LastModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success($"Payment \"{entity.SessionId}\" updated successfully", entity);

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { ex?.Message ?? ex?.InnerException?.Message
    });
            }

        }
    }
}
