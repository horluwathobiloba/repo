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

namespace OnyxDoc.AuthService.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public RoleAccessLevel RoleAccessLevel { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(a => a.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Specified");
                }
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                //if (userCheck.user == null)
                //{
                //    return Result.Failure("User does not exist in this organisation");
                //}
                var role = new Role
                {
                    Name = request.Name,
                    CreatedByEmail = userCheck.user?.UserId,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    RoleAccessLevel = request.RoleAccessLevel,
                    RoleAccessLevelDesc = request.RoleAccessLevel.ToString(),
                    SubscriberId = request.SubscriberId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
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



