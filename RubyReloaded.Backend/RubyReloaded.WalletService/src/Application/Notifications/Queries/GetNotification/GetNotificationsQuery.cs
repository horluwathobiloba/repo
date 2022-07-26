using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Queries.GetNotification
{
    public class GetNotificationsQuery : IRequest<Result>
    {
    }

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNotificationsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _context.Notifications.ToListAsync();

                if (notifications == null || notifications.Count == 0)
                {
                    return Result.Failure("Notifications do not exist");
                }
               // var notificationsList = _mapper.Map<List<NotificationListDto>>(notifications);
                return Result.Success("", notifications);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error getting notifications", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
