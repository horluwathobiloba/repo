using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;

namespace Onyx.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<Result>
    {
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
                var result = await _identityService.GetAll();
                if (result.staffs == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users" });
                }
                var roles = await _context.Roles.ToListAsync();
                var jobFunctions = await _context.JobFunctions.ToListAsync();
                foreach (var staff in result.staffs)
                {
                    staff.Role = roles.FirstOrDefault(a => a.Id == staff.RoleId);
                    staff.JobFunction = jobFunctions.FirstOrDefault(a => a.Id == staff.JobFunctionId);
                }
                var staffList = _mapper.Map<List<UserListDto>>(result.staffs);
                return Result.Success(staffList);
                  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
