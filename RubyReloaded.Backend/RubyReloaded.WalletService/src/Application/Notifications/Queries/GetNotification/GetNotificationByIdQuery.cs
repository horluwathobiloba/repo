using AutoMapper;
using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Notifications.Queries.GetNotification
{
    public class GetNotificationByIdQuery : IRequest<Result>
    {
        public string Id { get; set; }
    }


    public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNotificationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(request.Id);
                if (notification == null)
                {
                    return Result.Failure("No Notification not found with this id");
                }
            //    var notificationVm = _mapper.Map<NotificationDto>(notification);
                return Result.Success("", notification);

            }
            catch (Exception ex)
            {
                return Result.Failure("Error getting notifications by Id :"+ ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
