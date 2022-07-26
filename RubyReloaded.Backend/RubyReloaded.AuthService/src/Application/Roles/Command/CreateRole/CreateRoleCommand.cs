using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Roles.Command.CreateRole
{
    public class CreateRoleCommand:IRequest<Result>
    {
        public int CooperativeId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public AccessLevel AccessLevel { get; set; }
      // public int RoleId { get; set; }
          
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
                var checkRoles = await _context.Roles.Where(a => a.CooperativeId == request.CooperativeId).ToListAsync();


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

                var cooperative = await _context.Cooperatives.FirstOrDefaultAsync(a => a.Id == request.CooperativeId);
              
                var role = new CooperativeRole
                {
                    Name = request.Name.Trim(),
                    Cooperative =cooperative,
                    Status = Status.Active,
                    AccessLevel = request.AccessLevel,
                    CooperativeId=request.CooperativeId,
                    StatusDesc = Status.Active.ToString(),
                    AccessLevelDesc = request.AccessLevel.ToString()
                };
                await _context.Roles.AddAsync(role);
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
