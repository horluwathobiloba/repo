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
    public class GetUsersQuery : IRequest<Result>
    {
        public string UserId { get; set; }
        public int SubscriberId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetAll(request.Skip, request.Take);
                if (result.users == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users" });
                }
                var roles = await _context.Roles.ToListAsync();
                foreach (var user in result.users)
                {
                    user.Role = roles.FirstOrDefault(a => a.Id == user.RoleId);
                }
                var userList = _mapper.Map<List<UserListDto>>(result.users);
                return Result.Success(userList.OrderBy(a=>a.CreatedDate));
                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
