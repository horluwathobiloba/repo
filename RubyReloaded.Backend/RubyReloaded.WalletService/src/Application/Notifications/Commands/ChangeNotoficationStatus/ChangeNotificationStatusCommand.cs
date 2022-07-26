using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.ChangeNotoficationStatus
{
    public class ChangeNotificationStatusCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string DeviceId { get; set; }
        public string CustomerId { get; set; }
    }

    public class ChangeNotificationStatusCommandHandler : IRequestHandler<ChangeNotificationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ChangeNotificationStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(request.Id);
                if (notification == null)
                {
                    return Result.Failure(new string[] { "Invalid Notification for status change" });
                };
                notification.NotificationStatus = request.NotificationStatus;
                notification.ApplicationType = request.ApplicationType;
                notification.DeviceId = request.DeviceId;
                _context.Notifications.Update(notification);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Notification status updated successfully", notification);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Notification status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
