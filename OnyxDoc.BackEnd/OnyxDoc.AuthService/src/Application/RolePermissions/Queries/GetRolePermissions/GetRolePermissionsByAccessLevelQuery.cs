using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using System.Collections.Generic;
using OnyxDoc.AuthService.Application.Common.Models;
using System.Text.RegularExpressions;

namespace OnyxDoc.AuthService.Application.RolePermissions.Queries.GetRolePermissions
{
    public class GetRolePermissionsByAccessLevelQuery : IRequest<Result>
    {
        public int  AccessLevel { get; set; }
    }

    public class GetRoleByAccessLevelQueryHandler : IRequestHandler<GetRolePermissionsByAccessLevelQuery, Result>
    {
        private readonly IApplicationDbContext _context;

        public GetRoleByAccessLevelQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(GetRolePermissionsByAccessLevelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var permissionsList = new List<string>();
                //var commaSeperatedPermissions = new List<string>();
                //switch ((AccessLevel)request.AccessLevel)
                //{
                //    case AccessLevel.SuperAdmin:
                //        permissionsList = Enum.GetNames(typeof(SuperAdminPermissions)).ToList();
                //        foreach (var permission in permissionsList)
                //        {
                //            commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                //        }
                //        return Result.Success(commaSeperatedPermissions);

                //    case AccessLevel.PowerUser:
                //        permissionsList = Enum.GetNames(typeof(PowerUsersPermissions)).ToList();
                //        foreach (var permission in permissionsList)
                //        {
                //            commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                //        }
                //        return Result.Success(commaSeperatedPermissions);

                //    case AccessLevel.Admin:
                //        permissionsList = Enum.GetNames(typeof(AdminPermissions)).ToList();
                //        foreach (var permission in permissionsList)
                //        {
                //            commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                //        }
                //        return Result.Success(commaSeperatedPermissions);

                //    case AccessLevel.ExternalUser:
                //        permissionsList = Enum.GetNames(typeof(ExternalUserPermissions)).ToList();
                //        foreach (var permission in permissionsList)
                //        {
                //            commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                //        }
                //        return Result.Success(commaSeperatedPermissions);


                //    case AccessLevel.Support:
                //        permissionsList = Enum.GetNames(typeof(SupportPermissions)).ToList();
                //        foreach (var permission in permissionsList)
                //        {
                //            commaSeperatedPermissions.Add(Regex.Replace(permission, "([A-Z])", " $1").Trim());
                //        }
                     return Result.Success();

                //    default:
                //        return Result.Failure(new string[] { "Unsupported access level"});
                //}
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving role permissions", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
