using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Domain.Enums;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Queries.GetNotification
{
    public class GetUnReadNotificationsByUserIdQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public string DeviceNotificationId { get; set; }
    }
    public class GetUnReadNotificationsByUserIdQueryHandler : IRequestHandler<GetUnReadNotificationsByUserIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetUnReadNotificationsByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetUnReadNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var unReadNotifications = await _context.Notifications
                    .Where(a => a.UserId == request.UserId &&
                    a.NotificationStatus == NotificationStatus.Unread)
                    .OrderByDescending(x => x.CreatedDate.Year)
                .ThenByDescending(x => x.CreatedDate.Month)
                .ThenByDescending(x => x.CreatedDate.Day)
                .ToListAsync();
                return Result.Success(unReadNotifications);

            }
            catch (Exception ex)
            {
                return Result.Failure("Error getting unread notifications: " + ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
