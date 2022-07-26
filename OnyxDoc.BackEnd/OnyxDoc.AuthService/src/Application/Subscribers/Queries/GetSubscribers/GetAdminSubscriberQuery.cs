using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Application.Users.Queries.GetUsers;

namespace OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers
{
    public class GetAdminSubscriberQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
    }

    public class GetAdminSubscriberQueryHandler : IRequestHandler<GetAdminSubscriberQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetAdminSubscriberQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetAdminSubscriberQuery request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.UserId == "null" || request.UserId == "0")
                {
                    request.UserId = null;
                }
                if (!string.IsNullOrWhiteSpace(request.UserId))
                {
                    var result = await _identityService.GetUserById(request.UserId.Trim());
                    var user = _mapper.Map<UserDto>(result.user);
                    if (user == null)
                    {
                        return Result.Failure(new string[] { $"User does not exist with Id: {request.Id}" });
                    }
                    if (request.Id == 0)
                    {
                        request.Id = user.SubscriberId;
                    }
                    var subscriberDetail = await _context.Subscribers.Where(a => a.Id == user.SubscriberId && a.SubscriberAccessLevel == Domain.Enums.SubscriberAccessLevel.System).FirstOrDefaultAsync();
                    if (subscriberDetail != null)
                    {
                        var userSubscriberDetail = _mapper.Map<SubscriberDto>(subscriberDetail);
                        return Result.Success(userSubscriberDetail);
                    }
                  
                }
               
                var subscriber = await _context.Subscribers.Where(a => a.Id == request.Id && a.SubscriberAccessLevel == Domain.Enums.SubscriberAccessLevel.System).FirstOrDefaultAsync();
                var subscriberDetails = _mapper.Map<SubscriberDto>(subscriber);
                if (subscriberDetails == null)
                {
                   // return Result.Failure(new string[] { $"Subscriber Details does not exist with Id: {request.Id}" });
                }
                
                return Result.Success(subscriberDetails);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving subscriber details by Id: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
