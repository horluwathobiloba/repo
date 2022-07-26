using Onyx.AuthService.Application.Common.Exceptions;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Onyx.AuthService.Domain.ViewModels;

namespace Onyx.AuthService.Application.RolePermissions.Commands.UpdateRoleAndPermissions
{
    public partial class UpdateRoleAndPermissionsCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Status Status { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public List<RolePermissionsVm> RolePermissions { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleAndPermissionsCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;


        public UpdateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(UpdateRoleAndPermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to create role.Invalid User ID and Organization credentials!" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid role permissions", });
                }
                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Id == request.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Invalid organization details" });
                }
              
                await _context.BeginTransactionAsync();

                var getRoleForUpdate = await _context.Roles.FirstOrDefaultAsync(a => a.Id == request.RoleId && a.OrganizationId == request.OrganizationId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedBy = user.staff.UserName;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.AccessLevel = request.AccessLevel;
                getRoleForUpdate.AccessLevelDesc = request.AccessLevel.ToString();
                await _context.BeginTransactionAsync();
                _context.Roles.Update(getRoleForUpdate);

                var getRolePermissions = await _context.RolePermissions.Where(a => a.RoleId == request.RoleId).ToListAsync();
                if (getRolePermissions == null)
                {
                    return Result.Failure(new string[] { "Invalid Role Permissions for update" });
                }
                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "No Role Permissions for update", });
                }
                foreach (var item in request.RolePermissions)
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
                        _context.RolePermissions.Update(permissionForUpdate);
                    }
                }
                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return Result.Success("Role and Permissions created successfully", new { Role = getRoleForUpdate , Permissions = request.RolePermissions });
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message + ex?.InnerException.Message });
            }
        }
    }
}
