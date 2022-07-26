using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Application.RolePermissions.Queries.GetRolePermissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRolePermission.UpdateAjoRolePermission
{
    public class UpdateAjoRolePermissionCommand:IRequest<Result>
    {
        public int RoleId { get; set; }
        public int RolePermissionId { get; set; }
        public Dictionary<string, List<RolePermissionsVm>> RolePermissions { get; set; }
        
    }

    public class UpdateAjoRolePermissionCommandHandler : IRequestHandler<UpdateAjoRolePermissionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public UpdateAjoRolePermissionCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
        public async Task<Result> Handle(UpdateAjoRolePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Role Permissions
                var getRolePermissions = await _context.AjoRolePermissions.Where(a => a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "No Role Permissions for update", });
                }
                foreach (var permission in request.RolePermissions)
                {
                    foreach (var item in permission.Value)
                    {
                        if (getRolePermissions.Any(a => a.Permission == item.Permission.ToString() &&
                                   a.Status == item.Status))
                        {
                            continue;
                        }
                        var permissionForUpdate = getRolePermissions.FirstOrDefault(a => a.Permission == item.Permission && a.Status != item.Status);
                        if (permissionForUpdate != null)
                        {
                            permissionForUpdate.Status = item.Status;
                            _context.AjoRolePermissions.Update(permissionForUpdate);
                        }
                    }
                }
                return Result.Success("Role and Permissions created successfully");

            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message ?? ex?.InnerException.Message }); throw;
            }
        }
    }
}
