using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Roles.Commands.ChangeRoleStatus
{
    public class ChangeRolePermissionCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public SuperAdminPermissions SuperAdminPermissions { get; set; }
        public AdminPermissions AdminPermissions { get; set; }
        public PowerUsersPermissions PowerUsersPermissions { get; set; }
        public ExternalUserPermissions ExternalUserPermissions { get; set; }
        public SupportPermissions SupportPermissions { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Status Status { get; set; }

        //public class  ChangeRolePermissionCommandHandler : IRequestHandler<ChangeRolePermissionCommand, Result>
        //{
        //    private readonly IApplicationDbContext _context;
        //    private readonly IIdentityService _identityService;
        //    public ChangeRolePermissionCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        //    {
        //        _context = context;
        //        _identityService = identityService;
        //    }
        //    public async Task<Result>Handle(ChangeRolePermissionCommand request, CancellationToken cancellatiionToken)
        //    {
        //        try
        //        {
        //            var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
        //            if (user.staff == null)
        //            {
        //                return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Organization credentials!" });
        //            }
        //            var getRoleForUpdate = await _context.Roles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
        //            if (getRoleForUpdate == null)
        //            {
        //                return Result.Failure(new string[] { "Invalid Role" });
        //            }
        //            getRoleForUpdate.Name = request.Name.Trim();
        //            getRoleForUpdate.LastModifiedBy = user.staff.UserName;
        //            getRoleForUpdate.LastModifiedDate = DateTime.Now;
        //            getRoleForUpdate.Status = request.Status;

        //            var permissionsList = new List<string>();

        //            permissionsList = Enum.GetNames(typeof(SuperAdminPermissions)).ToList();
                   
        //            foreach (var item in permissionsList)
        //            {
        //                getRoleForUpdate.SuperAdminPermissions = request.SuperAdminPermissions;
        //            }

        //            _context.Roles.Update(getRoleForUpdate);
        //            await _context.SaveChangesAsync(cancellatiionToken);

        //            return Result.Success("Role updated successfully", getRoleForUpdate);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }


        //}
    }


    
}
