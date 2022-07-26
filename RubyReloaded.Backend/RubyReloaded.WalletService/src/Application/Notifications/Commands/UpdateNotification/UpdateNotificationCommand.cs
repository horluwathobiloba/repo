using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.UpdateNotification
{
    public partial class UpdateNotificationCommand : IRequest<Result>
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
    }

    public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateNotificationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var getExistingNotification = await _context.Notifications.FindAsync(request.NotificationId);
                if (getExistingNotification == null)
                {
                    return Result.Failure("Notification does not exist with the specified Id");
                }
                getExistingNotification.NotificationStatus = request.NotificationStatus;
                getExistingNotification.DeviceId = request.DeviceId;
                getExistingNotification.ApplicationType = request.ApplicationType;
                getExistingNotification.CreatedBy = request.UserId;
                getExistingNotification.DeviceId = request.DeviceId;

                _context.Notifications.Update(getExistingNotification);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Notification details updated successfully", getExistingNotification);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating notifications", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
