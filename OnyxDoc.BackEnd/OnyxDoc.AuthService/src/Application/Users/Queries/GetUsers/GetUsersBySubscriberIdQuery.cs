using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUsersBySubscriberIdQuery : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetUsersBySubscriberIdQueryHandler : IRequestHandler<GetUsersBySubscriberIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersBySubscriberIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUsersBySubscriberIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUsersBySubscriberId(request.SubscriberId,  request.Take, request.Skip);
                if (result.users == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users in this subscriber: {request.SubscriberId}" });
                }
                var roles = await _context.Roles.Where(a => a.SubscriberId == request.SubscriberId).ToListAsync();
                foreach (var user in result.users)
                {
                    user.Role = roles.FirstOrDefault(a => a.Id == user.RoleId);
                    //user.JobFunction = jobFunctions.FirstOrDefault(a => a.Id == user.JobFunctionId);
                }
                var userList = _mapper.Map<List<UserListDto>>(result.users);
                return Result.Success(new { users = userList.OrderBy(a=>a.CreatedDate) , Total = userList.Count });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving users by subscriber: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
