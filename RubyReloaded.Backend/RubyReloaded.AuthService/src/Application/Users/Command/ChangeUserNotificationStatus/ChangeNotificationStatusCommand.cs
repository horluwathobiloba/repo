using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.ChangeUserNotificationStatus
{
    public class ChangeNotificationStatusCommand:IRequest<Result>
    {
        public string UserId { get; set; }
       
    }
    public class ChangeUserStatusHandler : IRequestHandler<ChangeNotificationStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public ChangeUserStatusHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {  
                var result = await _identityService.ChangeUserNotificationStatusAsync(request.UserId);
                await _context.SaveChangesAsync(cancellationToken);
                if (result.Succeeded)
                    return Result.Success("NotificationStatus Update Successful");
                else
                    return Result.Failure(result.Messages);
            }
            catch (Exception ex)
            {
                return Result.Failure("User status change was not successful :"+ ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
