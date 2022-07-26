using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Entities;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Commands.CreateNotification
{
    public class CreateNotificationCommand:IRequest<Result>
    {
        public string Message { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public CreateNotificationCommandHandler(IApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _notificationService.SendNotification(request.DeviceId, request.Message);

                var notification = new Notification
                {
                    ApplicationType = request.ApplicationType,
                    NotificationStatus = NotificationStatus.Unread,
                    UserId = request.UserId,
                    Message = request.Message,
                    CreatedBy = request.UserId,
                    CreatedDate = DateTime.Now,
                    DeviceId = request.DeviceId,
                    Status = Status.Active
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Notification created successfully", notification);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Notification creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }
    }
}
