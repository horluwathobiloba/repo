using Onyx.WorkFlowService.Application.Common.Interfaces;
using Onyx.WorkFlowService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Onyx.WorkFlowService.Domain.Enums;
using System;
using Onyx.WorkFlowService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RestSharp;
using System.Collections.Generic;

namespace Onyx.WorkFlowService.Application.RolePermissions.Commands.CreateRolePermissions
{
    public class CreateRolePermissionsCommand : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public List<string> RolePermissions { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRolePermissionsCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Organization credentials!" });
                }

                if (request.RolePermissions == null || request.RolePermissions.Count == 0)
                {
                    return Result.Failure(new string[] { "Please input valid Role Permissions", });
                }
                foreach (var permission in request.RolePermissions)
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
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role Permission creation was successful");
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Role Permission creation was not successful", ex?.Message??ex?.InnerException.Message });
            }

        }
    }
}
