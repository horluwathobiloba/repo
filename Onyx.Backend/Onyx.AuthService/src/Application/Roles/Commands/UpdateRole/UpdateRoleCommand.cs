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

namespace Onyx.AuthService.Application.Roles.Commands.UpdateRole
{
    public partial class UpdateRoleCommand :  IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public WorkflowUserCategory WorkflowUserCategory { get; set; }
        public Status Status { get; set; }

    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateRoleCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }
        

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndOrganization(request.UserId, request.OrganizationId);
                if (user.staff == null)
                {
                    return Result.Failure(new string[] { "Unable to update role.Invalid User ID and Organization credentials!" });
                }
                var getRoleForUpdate = await _context.Roles.FirstOrDefaultAsync(a=>a.Id == request.RoleId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedBy = user.staff.UserName;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.AccessLevel = request.AccessLevel;
                getRoleForUpdate.WorkflowUserCategory = request.WorkflowUserCategory;
                getRoleForUpdate.Status = request.Status;
                getRoleForUpdate.StatusDesc = request.Status.ToString();

                 _context.Roles.Update(getRoleForUpdate);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Role updated successfully", getRoleForUpdate);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating role: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
