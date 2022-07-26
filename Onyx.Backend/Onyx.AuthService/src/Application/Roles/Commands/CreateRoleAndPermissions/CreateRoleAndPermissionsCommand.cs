using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Domain.ViewModels;

namespace Onyx.AuthService.Application.Roles.Commands.CreateRoleAndPermissions
{
    public class CreateRoleAndPermissionsCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public List<RolePermissionsVm> RolePermissions { get; set; }
    }

    public class CreateRoleAndPermissionsCommandHandler : IRequestHandler<CreateRoleAndPermissionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateRoleAndPermissionsCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }
      
        public async Task<Result> Handle(CreateRoleAndPermissionsCommand request, CancellationToken cancellationToken)
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
                var roledetails = await _context.Roles.FirstOrDefaultAsync(a => a.Name == request.Name && a.OrganizationId == request.OrganizationId);
                if (roledetails != null)
                {
                    return Result.Failure(new string[] { "Role details already exist" });
                }
                await _context.BeginTransactionAsync();
                var role = new Role
                {
                    Name = request.Name.Trim(),
                    OrganizationId = request.OrganizationId,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    WorkflowUserCategory = request.WorkflowUserCategory,
                    WorkflowUserCategoryDesc = request.WorkflowUserCategory.ToString(),
                    CreatedBy = user.staff.Email,
                    CreatedById = user.staff.UserId,
                    CreatedDate = DateTime.Now,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);

                var rolePermissionsList = new List<RolePermissionsDT>();
                if (role.Id != 0)
                {
                    foreach (var permission in request.RolePermissions)
                    {
                        var rolePermission = new RolePermissionsDT
                        {

                            OrganizationId = request.OrganizationId,
                            AccessLevel = (int)request.AccessLevel,
                            Permission = permission.Permission.Trim(),
                            RoleId = role.Id,
                            CreatedBy = user.staff.Email,
                            CreatedById = user.staff.UserId,
                            CreatedDate = DateTime.Now,
                            Status = (int)permission.Status
                        };
                        rolePermissionsList.Add(rolePermission);
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
                await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                await _context.CommitTransactionAsync();
                return Result.Success("Role and Permissions created successfully", new { role , rolePermissionsList });
            }
            catch (Exception ex)
            {
                _context.RollbackTransaction();
                return Result.Failure(new string[] { "Role and Permissions creation was not successful", ex?.Message + ex?.InnerException.Message });
            }

        }
    }
}
