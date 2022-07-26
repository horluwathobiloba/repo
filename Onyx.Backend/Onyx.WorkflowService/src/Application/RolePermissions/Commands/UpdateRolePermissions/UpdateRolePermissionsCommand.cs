using Onyx.WorkFlowService.Application.Common.Exceptions;
using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Onyx.WorkFlowService.Domain.Enums;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Onyx.WorkFlowService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public partial class UpdateRolePermissionsCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public List<string> RolePermissions { get; set; }

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
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
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
                    var permission = item.Trim();
                    if (getRolePermissions.Any(a => a.Permission != permission))
                    {
                        var rolePermissions = new RolePermission
                        {

                            OrganizationId = request.OrganizationId,
                            AccessLevel = request.AccessLevel,
                            Permission = permission.Trim(),
                            RoleId = request.RoleId,
                            CreatedBy = request.UserId,
                            CreatedDate = DateTime.Now,
                            Status = Status.Active
                        };
                        await _context.RolePermissions.AddAsync(rolePermissions);
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
