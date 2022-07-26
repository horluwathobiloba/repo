using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoRole.Commands.UpdateAjoRole
{
    public class UpdateAjoRoleCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public Status Status { get; set; }
    }
    public class UpdateAjoRoleCommandHandler : IRequestHandler<UpdateAjoRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public UpdateAjoRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        public async Task<Result> Handle(UpdateAjoRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var getRoleForUpdate = await _context.AjoRoles.FirstOrDefaultAsync(a => a.Id == request.RoleId);
                if (getRoleForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Role" });
                }
                getRoleForUpdate.Name = request.Name.Trim();
                getRoleForUpdate.LastModifiedById = request.UserId;
                getRoleForUpdate.LastModifiedDate = DateTime.Now;
                getRoleForUpdate.AccessLevel = request.AccessLevel;
                getRoleForUpdate.Status = request.Status;
                getRoleForUpdate.AccessLevelDesc = request.AccessLevel.ToString();
                getRoleForUpdate.Status = request.Status;
                getRoleForUpdate.StatusDesc = request.Status.ToString();
                _context.AjoRoles.Update(getRoleForUpdate);
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
