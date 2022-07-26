using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;

namespace Onyx.AuthService.Application.Users.Queries.GetUsers
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
                if (result.staff == null)
                {
                    return Result.Failure(new string[] { $"Invalid User" });
                }
                var staff = _mapper.Map<UserDto>(result.staff);
                staff.Role = await _context.Roles.Where(a => a.Id == staff.RoleId).FirstOrDefaultAsync();
                staff.Organization = await _context.Organizations.Where(a => a.Id == staff.OrganizationId).FirstOrDefaultAsync();
                staff.JobFunction = await _context.JobFunctions.Where(a => a.Id == staff.JobFunctionId).FirstOrDefaultAsync();
                return Result.Success(staff);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error retrieving staff by username: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
