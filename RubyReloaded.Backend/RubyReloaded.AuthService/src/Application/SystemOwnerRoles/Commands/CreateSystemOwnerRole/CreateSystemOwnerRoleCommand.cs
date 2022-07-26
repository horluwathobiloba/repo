using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SystemOwnerRoles.Commands.CreateSystemOwnerRole
{
    public class CreateSystemOwnerRoleCommand:IRequest<Result>
    {
        public int SystemOwnerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
       
    }
    public class CreateSystemOwnerRoleCommandHandler : IRequestHandler<CreateSystemOwnerRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ISqlService _sqlService;
        public CreateSystemOwnerRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService, ISqlService sqlService)
        {
            _context = context;
            _identityService = identityService;
            _sqlService = sqlService;
        }

        public async Task<Result> Handle(CreateSystemOwnerRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdBy = "";
                //check userid only if default users exist for an organization, this would bypass validation of user on admin creation
                var checkRoles = await _context.SystemOwnerRoles.Where(a => a.SystemOwnerId == request.SystemOwnerId).ToListAsync();


                var user = await _identityService.GetUserById(request.UserId);
                // we need a get user by Cooperative
                if (checkRoles.Any())
                {
                    if (user.user == null)
                    {
                        return Result.Failure(new string[] { "Unable to create role.Invalid User ID and Organization credentials!" });
                    }
                    else
                    {
                        createdBy = user.user.UserId;
                    }
                }
                else
                {
                    createdBy = user.user?.Email ?? request.CreatedBy;
                }

                var systemOwner = await _context.SystemOwners.FirstOrDefaultAsync(a => a.Id == request.SystemOwnerId);
                var systemRoleId = await _context.SystemOwnerRoles.Select(x => x.Id).MaxAsync();
                var role = new SystemOwnerRole
                {
                    //Id=systemRoleId+1,
                    Name = request.Name.Trim(),
                    SystemOwner = systemOwner,
                    Status = Status.Active,
                    AccessLevel = request.AccessLevel,
                    SystemOwnerId = request.SystemOwnerId,
                    StatusDesc = Status.Active.ToString(),
                    AccessLevelDesc = request.AccessLevel.ToString()
                };
                await _context.SystemOwnerRoles.AddAsync(role);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Role created successfully", role);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Role creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
