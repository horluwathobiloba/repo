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
    public class GetReadNotificationsByUserIdQuery:IRequest<Result>
    {
        public string UserId { get; set; }
        public string DeviceNotificationId { get; set; }
}

public class GetReadNotificationByUserIdQueryHandler : IRequestHandler<GetReadNotificationsByUserIdQuery, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
  //  private readonly IIdentityService _identityService;

    public GetReadNotificationByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
       
    }

    public async Task<Result> Handle(GetReadNotificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
           
            var readNotifications = await _context.Notifications.Where(a => a.UserId == request.UserId &&
            a.NotificationStatus == NotificationStatus.Read)
            .OrderByDescending(x => x.CreatedDate.Year)
            .ThenByDescending(x => x.CreatedDate.Month)
            .ThenByDescending(x => x.CreatedDate.Day)
            .ToListAsync();
            return Result.Success(readNotifications);
        }
        catch (Exception ex)
        {
            return Result.Failure("Error getting read notifications :"+ ex?.Message ?? ex?.InnerException?.Message );
        }
    }
}
}
