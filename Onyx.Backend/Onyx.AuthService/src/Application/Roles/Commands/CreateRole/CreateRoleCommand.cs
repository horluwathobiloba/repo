using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Domain.Enums;
using System;
using Onyx.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Onyx.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Onyx.AuthService.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                //check userid only if default users exist for an organization, this would bypass validation of user on admin creation
                var checkRoles = await _context.Roles.Where(a => a.OrganizationId == request.OrganizationId).ToListAsync();

              
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (checkRoles.Any())
                {
                    if (user.staff == null)
                    {
                        return Result.Failure(new string[] { "Unable to create role.Invalid User ID and Organization credentials!" });
                    }
                    else
                    {
                        createdBy = user.staff.UserId;
                    }
                }
                else
                {
                    createdBy = user.staff?.Email ?? request.CreatedBy;
                }

                var org = await _context.Organizations.FirstOrDefaultAsync(a => a.Id == request.OrganizationId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "User does not belong to Organization, so unable to create role." });
                }
                var roledetails = await _context.Roles.FirstOrDefaultAsync(a => a.Name == request.Name && a.OrganizationId == request.OrganizationId);
                if (roledetails != null)
                {
                    return Result.Failure(new string[] { "Role details already exist" });
                }
                var role = new Role
                {
                    Name = request.Name.Trim(),
                    OrganizationId = request.OrganizationId,
                    Status = Status.Active,
                    AccessLevel = request.AccessLevel,
                    WorkflowUserCategory = request.WorkflowUserCategory,
                    StatusDesc = Status.Active.ToString(),
                    AccessLevelDesc = request.AccessLevel.ToString(),
                   WorkflowUserCategoryDesc = request.WorkflowUserCategory.ToString()
                };
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);


                //Create initial role permissions for SuperAdmin on Organization setup

                var rolePermissionsList = new List<RolePermissionsDT>();
                var commaSeperatedPermissions = new List<string>();

                switch (request.AccessLevel)
                {

                    case AccessLevel.Admin:
                        var permissionsList = Enum.GetNames(typeof(AdminPermissions)).ToList();

                        foreach (var permission in permissionsList)
                        {
                            var rolePermission = new RolePermissionsDT
                            {
                                Name = role.Name,
                                OrganizationId = request.OrganizationId,
                                AccessLevel = (int)request.AccessLevel,
                                Permission = Regex.Replace(permission, "([A-Z])", " $1").Trim(),
                                RoleId = role.Id,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                Status = (int)Status.Active
                            };
                            rolePermissionsList.Add(rolePermission);
                        }
                        await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                        return Result.Success("Role created successfully", role, rolePermissionsList);
                        

                    case AccessLevel.SuperAdmin:
                        permissionsList = Enum.GetNames(typeof(SuperAdminPermissions)).ToList();

                        foreach (var permission in permissionsList)
                        {
                            var rolePermission = new RolePermissionsDT
                            {
                                Name = role.Name,
                                OrganizationId = request.OrganizationId,
                                AccessLevel = (int)request.AccessLevel,
                                Permission = Regex.Replace(permission, "([A-Z])", " $1").Trim(),
                                RoleId = role.Id,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                Status = (int)Status.Active
                            };
                            rolePermissionsList.Add(rolePermission);
                        }
                        await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                        return Result.Success("Role created successfully", role, rolePermissionsList);

                    case AccessLevel.PowerUser:
                         permissionsList = Enum.GetNames(typeof(PowerUsersPermissions)).ToList();

                        foreach (var permission in permissionsList)
                        {
                            var rolePermission = new RolePermissionsDT
                            {
                                Name = role.Name,
                                OrganizationId = request.OrganizationId,
                                AccessLevel = (int)request.AccessLevel,
                                Permission = Regex.Replace(permission, "([A-Z])", " $1").Trim(),
                                RoleId = role.Id,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                Status = (int)Status.Active
                            };
                            rolePermissionsList.Add(rolePermission);
                        }
                        await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                        return Result.Success("Role created successfully", role, rolePermissionsList);

                    case AccessLevel.ExternalUser:
                        permissionsList = Enum.GetNames(typeof(ExternalUserPermissions)).ToList();

                        foreach (var permission in permissionsList)
                        {
                            var rolePermission = new RolePermissionsDT
                            {
                                Name = role.Name,
                                OrganizationId = request.OrganizationId,
                                AccessLevel = (int)request.AccessLevel,
                                Permission = Regex.Replace(permission, "([A-Z])", " $1").Trim(),
                                RoleId = role.Id,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                Status = (int)Status.Active
                            };
                            rolePermissionsList.Add(rolePermission);
                        }
                        await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                        return Result.Success("Role created successfully", role, rolePermissionsList);

                    case AccessLevel.Support:
                        permissionsList = Enum.GetNames(typeof(SupportPermissions)).ToList();

                        foreach (var permission in permissionsList)
                        {
                            var rolePermission = new RolePermissionsDT
                            {
                                Name = role.Name,
                                OrganizationId = request.OrganizationId,
                                AccessLevel = (int)request.AccessLevel,
                                Permission = Regex.Replace(permission, "([A-Z])", " $1").Trim(),
                                RoleId = role.Id,
                                CreatedBy = createdBy,
                                CreatedDate = DateTime.Now,
                                Status = (int)Status.Active
                            };
                            rolePermissionsList.Add(rolePermission);
                        }
                        await _sqlService.InsertAdminPermissions("RolePermissions", rolePermissionsList);
                        return Result.Success("Role created successfully", role, rolePermissionsList);
                    default:
                        return Result.Failure(new string[] { "Unsupported access level" });
                       
                }

            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Role creation was not successful", ex?.Message + ex?.InnerException.Message });
            }

        }


    }
 }



