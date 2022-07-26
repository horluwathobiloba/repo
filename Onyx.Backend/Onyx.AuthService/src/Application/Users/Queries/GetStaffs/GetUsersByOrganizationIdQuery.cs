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
    public class GetUsersByOrganizationIdQuery : IRequest<Result>
    {
        public int OrganizationId { get; set; }
    }

    public class GetUsersByOrganizationIdQueryHandler : IRequestHandler<GetUsersByOrganizationIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersByOrganizationIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUsersByOrganizationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUsersByOrganizationId(request.OrganizationId);
                if (result.staffs == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users in this organization: {request.OrganizationId}" });
                }
                var roles = await _context.Roles.Where(a => a.OrganizationId == request.OrganizationId).ToListAsync();
                var jobFunctions = await _context.JobFunctions.Where(a => a.OrganisationId == request.OrganizationId).ToListAsync();
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
                return Result.Failure(new string[] { "Error retrieving staff by organization: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
