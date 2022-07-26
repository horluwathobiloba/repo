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
    public class GetUsersByDepartmentIdQuery : IRequest<Result>
    {
        public int DepartmentId { get; set; }
    }

    public class GetUsersByDepartmentIdQueryHandler : IRequestHandler<GetUsersByDepartmentIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUsersByDepartmentIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUsersByDepartmentIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUsersByDepartmentId(request.DepartmentId);
                if (result.staffs == null)
                {
                    return Result.Failure(new string[] { $"Invalid Users in this department: {request.DepartmentId}" });
                }
                var staffList = _mapper.Map<List<UserListDto>>(result.staffs);
                return Result.Success(staffList);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving staff by department: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
