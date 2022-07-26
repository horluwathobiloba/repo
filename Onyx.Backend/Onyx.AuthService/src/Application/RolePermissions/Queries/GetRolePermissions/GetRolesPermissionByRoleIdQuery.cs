using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Enums;
using Onyx.AuthService.Domain.Entities;
using System.Text.RegularExpressions;
using System;

namespace Onyx.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolesPermissionByRoleIdQuery : IRequest<Result>
    {
        public int RoleId { get; set; }
        public Status Status { get; set; }
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
            var rolePermissions = await  _context.RolePermissions.Where(a=>a.RoleId == request.RoleId).ToListAsync();
            if (rolePermissions.Count == 0)
            {
                return Result.Failure("Role Permissions does not exist");
            }
            AccessLevel accessLevel = AccessLevel.Admin;
            var organizationId = 0;
            var createdBy = "";
            var createdById = "";
            if (rolePermissions != null && rolePermissions.Count > 0)
            {
                accessLevel = rolePermissions.FirstOrDefault().AccessLevel;
                organizationId = rolePermissions.FirstOrDefault().OrganizationId;
                createdBy = rolePermissions.FirstOrDefault().CreatedBy;
                createdById = rolePermissions.FirstOrDefault().CreatedById;
            }
            var permissionsList = new List<string>();
            var commaSeperatedPermissions = new List<string>();
            switch (accessLevel)
            {
                case AccessLevel.SuperAdmin:
                    permissionsList = Enum.GetNames(typeof(SuperAdminPermissions)).ToList();
                    foreach (var permission in permissionsList)
                    {
                        commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                    }
                    break;
                case AccessLevel.PowerUser:
                    permissionsList = Enum.GetNames(typeof(PowerUsersPermissions)).ToList();
                    foreach (var permission in permissionsList)
                    {
                        commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                    }
                    break;
                case AccessLevel.Admin:
                    permissionsList = Enum.GetNames(typeof(AdminPermissions)).ToList();
                    foreach (var permission in permissionsList)
                    {
                        commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                    }
                    break;

                case AccessLevel.ExternalUser:
                    permissionsList = Enum.GetNames(typeof(ExternalUserPermissions)).ToList();
                    foreach (var permission in permissionsList)
                    {
                        commaSeperatedPermissions.Add( Regex.Replace(permission, "([A-Z])", " $1").Trim());
                    }
                    break;

                case AccessLevel.Support:
                    permissionsList = Enum.GetNames(typeof(SupportPermissions)).ToList();
                    foreach (var permission in permissionsList)
                    {
                        commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                    }
                    break;
                default:
                    return Result.Failure(new string[] { "Unsupported access level" });
             
            }
            var newRolePermissions = new List<RolePermission>();
            var existingPermissions = rolePermissions.Select(a => a.Permission).ToList();
            foreach (var permission in commaSeperatedPermissions)
            {
                if (!existingPermissions.Contains(permission))
                {
                    var newPermission = new RolePermission();
                    newPermission.RoleId = request.RoleId;
                    newPermission.CreatedDate = DateTime.Now;
                    newPermission.CreatedBy = createdBy;
                    newPermission.CreatedById = createdById;
                    newPermission.AccessLevel = accessLevel;
                    newPermission.Status = Status.Inactive;
                    newPermission.StatusDesc = Status.Inactive.ToString();
                    newPermission.OrganizationId = organizationId;
                    newPermission.Permission = permission;
                    newRolePermissions.Add(newPermission);
                }
            }

            await _context.RolePermissions.AddRangeAsync(newRolePermissions);
            await _context.SaveChangesAsync(cancellationToken);

            rolePermissions.AddRange(newRolePermissions);

            var roleList = _mapper.Map<List<RolePermissionListDto>>(rolePermissions);
            return Result.Success(roleList);
        }
    }
}
