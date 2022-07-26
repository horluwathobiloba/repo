using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.Common.Interfaces;

namespace RubyReloadedReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolePermissionsQuery : IRequest<Result>
    {
        public int CooperativeId { get; set; }
    }

    public class GetRolesQueryHandler : IRequestHandler<GetRolePermissionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rolePermissions = await _context.RolePermissions.Where(a=>a.CooperativeId == request.CooperativeId).ToListAsync();
                var permissions = _mapper.Map<List<RolePermissionListDto>>(rolePermissions);
                return Result.Success(rolePermissions);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving role permissions", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
