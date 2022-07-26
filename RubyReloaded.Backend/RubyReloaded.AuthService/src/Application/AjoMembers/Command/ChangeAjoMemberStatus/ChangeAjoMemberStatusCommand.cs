﻿using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoMembers.Command.ChangeAjoMemberStatus
{
    public class ChangeAjoMemberStatusCommand:IRequest<Result>
    {
        public int AjoMemberId { get; set; }
        public string UserId { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class ChangeAjoMemberStatusCommandHandler : IRequestHandler<ChangeAjoMemberStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        //  private readonly IIdentityService _identityService;
        public ChangeAjoMemberStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeAjoMemberStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string message = "";
                //var user = await _context.AjoUserMappings.FirstOrDefaultAsync(x => x.AjoId == request.AjoId && x.UserId == request.UserId && x.RoleId == 1);
                //if (user == null)
                //{
                //    return Result.Failure(new string[] { "Invalid User for this operation" });
                //}
                var entity = await _context.AjoMembers.FindAsync(request.AjoMemberId);
                if (entity == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                switch (entity.Status)
                {
                    case Domain.Enums.Status.Active:
                        entity.Status = Domain.Enums.Status.Inactive;
                        message = "Ajo Member deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Ajo Member activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        entity.Status = Domain.Enums.Status.Active;
                        message = "Ajo Member activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Ajo Member status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
