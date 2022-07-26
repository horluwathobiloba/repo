using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.MakePayment.Commands
{
    public class MakePaymentCommand : IRequest<Result>
    {
        public ProvidusMakePaymentRequest ProvidusMakePaymentRequest { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }

    public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, Result>
    {
        private readonly IProvidusBankService _providus;
        private readonly INotificationService _notificationService;
        private readonly IApplicationDbContext _context;
        public MakePaymentCommandHandler(IProvidusBankService providus, INotificationService notificationService, IApplicationDbContext context)
        {
            _providus = providus;
            _notificationService = notificationService;
            _context = context;
        }
        public async Task<Result> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var message = "Payment Successful";
                var paymentStatus = await _providus.MakePayment(request.ProvidusMakePaymentRequest);
                if (paymentStatus is null || paymentStatus.responseCode != "00")
                {
                    return Result.Failure("Payment Operation failed");
                }

                var notification = new Notification
                {
                    ApplicationType = request.ApplicationType,
                    NotificationStatus = NotificationStatus.Unread,
                    Message = message,
                    Status = Status.Active,
                    DeviceId = request.DeviceId,
                    UserId = request.UserId,
                    CreatedBy = request.UserId
                };
                await _notificationService.SendNotification(request.DeviceId, message);
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(paymentStatus);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Making payment categories was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
