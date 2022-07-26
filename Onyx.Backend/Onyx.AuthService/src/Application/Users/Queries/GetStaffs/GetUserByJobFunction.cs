using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Application.Users.Queries.GetUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Onyx.AuthService.Application.Users.Queries.GetStaffs
{
    public class GetUserByJobFunction:IRequest<Result>
    {
        public int? OrgId { get; set; }
        public int? JobfunctionId { get; set; }
    }

    public class GetUserByJobFunctionQueryHandler : IRequestHandler<GetUserByJobFunction, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        public GetUserByJobFunctionQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<Result> Handle(GetUserByJobFunction request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUsersByJobFunctionAndOrganization(request.JobfunctionId, request.OrgId);
                if (result.users == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users for this jobfunction: {request.OrgId}" });
                }
                foreach (var staff in result.users)
                {
                    staff.Role = await _context.Roles.Where(a => a.Id == staff.RoleId).FirstOrDefaultAsync();
                }
                var staffList = _mapper.Map<List<UserListDto>>(result.users);
                return Result.Success(staffList);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving staff by Jobfunction: ", ex?.Message ?? ex?.InnerException?.Message });
            }
           
        }
    }
}
