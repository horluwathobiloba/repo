using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OnyxDoc.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OnyxDoc.AuthService.Application.Roles.Queries.GetRoles;

namespace OnyxDoc.AuthService.Application.Roless.Commands.CreateRoles
{
    public class CreateRolesCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public List<RolesVm> RolesVm { get; set; }
    }

    public class CreateRolesCommandHandler : IRequestHandler<CreateRolesCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRolesCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateRolesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(a => a.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Specified");
                }
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (userCheck.user is null)
                {
                    return Result.Failure("User does not exist in this organisation");
                }
                List<Role> roles = new List<Role>();
                foreach (var roleVm in request.RolesVm)
                {
                    var role = new Role
                    {
                        Name = roleVm.Name,
                        CreatedByEmail = userCheck.user.UserId,
                        CreatedById = request.UserId,
                        CreatedDate = DateTime.Now,
                        RoleAccessLevel = roleVm.RoleAccessLevel,
                        RoleAccessLevelDesc = roleVm.RoleAccessLevel.ToString(),
                        SubscriberId = request.SubscriberId,
                        Status = Status.Active,
                        StatusDesc = Status.Active.ToString()
                    };
                    roles.Add(role);
                }
              

                await _context.Roles.AddRangeAsync(roles);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Roles created successfully", roles);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Roles creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }


    }
}



