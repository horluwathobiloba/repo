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

namespace Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{ 
    public class GetRolePermissionsQuery : IRequest<Result>
    {
        public int OrganizationId { get; set; }
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
                var rolePermissions = await _context.RolePermissions.ToListAsync();
              
                var permissions = _mapper.Map<List<RolePermissionListDto>>(rolePermissions);
                return Result.Success(permissions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
