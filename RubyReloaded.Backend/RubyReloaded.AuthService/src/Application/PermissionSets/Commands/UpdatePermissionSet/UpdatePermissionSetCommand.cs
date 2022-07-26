using RubyReloaded.AuthService.Application.Common.Exceptions;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.UpdatePermissionSet
{
    public partial class UpdatePermissionSetCommand :  IRequest<Result>
    {
        public int PermissionSetId { get; set; }
        public string UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }

    }

    public class UpdatePermissionSetCommandHandler : IRequestHandler<UpdatePermissionSetCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdatePermissionSetCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }
        

        public async Task<Result> Handle(UpdatePermissionSetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update feature.Invalid User ID credentials!" });
                }
                var getPermissionSetForUpdate = await _context.PermissionSets.FirstOrDefaultAsync(a=>a.Id == request.PermissionSetId);
                if (getPermissionSetForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid PermissionSet" });
                }
                getPermissionSetForUpdate.Name = request.Name.Trim();
                getPermissionSetForUpdate.ParentID = request.ParentID;
                getPermissionSetForUpdate.ParentName = request.ParentName;
                getPermissionSetForUpdate.LastModifiedById = request.UserId;
                getPermissionSetForUpdate.LastModifiedDate = DateTime.Now;
                getPermissionSetForUpdate.AccessLevel = request.AccessLevel;
                getPermissionSetForUpdate.AccessLevelDesc = request.AccessLevel.ToString();

                _context.PermissionSets.Update(getPermissionSetForUpdate);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("PermissionSet updated successfully", getPermissionSetForUpdate);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating feature: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
