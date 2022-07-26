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
    public class GetUserByIdQuery : IRequest<Result>
    {
        public string  Id { get; set; }

        public int SubscriberId { get; set; }

        public string UserId { get; set; }
    }

    public class GetUserByIdQueryQueryHandler : IRequestHandler<GetUserByIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUserByIdQueryQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.Id.Trim());
                var user = _mapper.Map<UserDto>(result.user);
                if (user == null)
                {
                    return Result.Failure(new string[] { $"User does not exist with Id: {request.Id}" });
                }
                user.Role = await _context.Roles.Where(a => a.Id == user.RoleId).FirstOrDefaultAsync();
                user.Subscriber = await _context.Subscribers.Where(a => a.Id == user.SubscriberId).FirstOrDefaultAsync();
                return Result.Success(user);
                

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving user by Id: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
