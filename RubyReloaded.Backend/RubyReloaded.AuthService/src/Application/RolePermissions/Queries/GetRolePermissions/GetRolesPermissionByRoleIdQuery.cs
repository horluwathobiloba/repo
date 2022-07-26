using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using System.Linq;
using System.Collections.Generic;
using RubyReloaded.AuthService.Application.Common.Models;
using System;

namespace RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolesPermissionByRoleIdQuery : IRequest<Result>
    {
        public int RoleId { get; set; }
    }

    public class GetRolePermissionsByRoleIdHandler : IRequestHandler<GetRolesPermissionByRoleIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRolePermissionsByRoleIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(GetRolesPermissionByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _context.RolePermissions.FirstOrDefaultAsync(x => x.Id == request.RoleId);
                if (role is null)
                {
                    return Result.Failure("Invalid role Id");
                }
                var permissions = await _context.RolePermissions.Where(x => x.RoleId == role.Id).ToListAsync();
                var category = permissions.Select(a => a.Category).Distinct().ToList();
                var permissionsDictionary = new Dictionary<string, List<Domain.Entities.CooperativeRolePermission>>();
                foreach (var item in category)
                {
                    permissionsDictionary.Add(item, permissions.Where(a => a.Category == item).ToList());
                }
                return Result.Success(new { role, permissionsDictionary });
            }
            catch (Exception ex)
            {
                return Result.Failure("Retrieving roles and permissions by Role Id was not successful" + ex?.Message ?? ex?.InnerException?.Message);
            }
        }
    }
}
