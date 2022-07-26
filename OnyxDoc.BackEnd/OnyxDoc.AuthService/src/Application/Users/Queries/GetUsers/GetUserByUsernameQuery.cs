using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;

namespace OnyxDoc.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUserByUsernameQuery : IRequest<Result>
    {
        public string  UserName { get; set; }
    }

    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUserByUsernameQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserByUsername(request.UserName.Trim());
                if (result.user == null)
                {
                    return Result.Failure(new string[] { $"Invalid User" });
                }
                var user = _mapper.Map<UserDto>(result.user);
                user.Role = await _context.Roles.Where(a => a.Id == user.RoleId).FirstOrDefaultAsync();
                user.Subscriber = await _context.Subscribers.Where(a => a.Id == user.SubscriberId).FirstOrDefaultAsync();
                return Result.Success(user);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error retrieving user by username: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
