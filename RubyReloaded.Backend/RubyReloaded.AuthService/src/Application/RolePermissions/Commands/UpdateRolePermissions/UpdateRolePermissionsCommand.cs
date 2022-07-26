using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using RubyReloaded.AuthService.Domain.ViewModels;

namespace RubyReloaded.AuthService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public partial class UpdateRolePermissionsCommand :  IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public List<RolePermissionsVm> RolePermissions { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRolePermissionsCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
    

        public UpdateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Organization credentials!" });
                }
                var getRolePermissions = await _context.RolePermissions.Where(a=>a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid Role Permissions", });
                }
                 foreach (var item in request.RolePermissions)
                 {
                    if (getRolePermissions.Any(a => a.Permission == item.Permission.ToString() && 
                    a.Status == item.Status))
                    {
                        continue;
                    }
                    //if (getRolePermissions.Any(a => a.Permission != permission))
                    //{
                    //    var rolePermissions = new RolePermission
                    //    {

                    //        OrganizationId = request.OrganizationId,
                    //        AccessLevel = request.AccessLevel,
                    //        Permission = permission.Trim(),
                    //        RoleId = request.RoleId,
                    //        CreatedBy = request.UserId,
                    //        CreatedDate = DateTime.Now,
                    //        Status = item.Status
                    //    };
                    //    await _context.RolePermissions.AddAsync(rolePermissions);
                    //}
                    var permissionForUpdate = getRolePermissions.FirstOrDefault(a => a.Permission == item.Permission && a.Status != item.Status);
                    if (permissionForUpdate != null)
                    {
                        permissionForUpdate.Status = item.Status;
                         _context.RolePermissions.Update(permissionForUpdate);
                    }       
                 }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permissions updated successfully");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error updating role permissions: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
