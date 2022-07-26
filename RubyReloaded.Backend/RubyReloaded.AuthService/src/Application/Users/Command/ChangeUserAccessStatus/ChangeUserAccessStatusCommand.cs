using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Command.ChangeUserAccessStatus
{
    public class ChangeUserAccessStatusCommand:IRequest<Result>
    {
        public string UserId { get; set; }
        public int CooperativeId { get; set; }
        public CooperativeAccessStatus CooperativeAccessStatus { get; set; }
    }
    public class ChangeUserAccessStatusCommandHandler : IRequestHandler<ChangeUserAccessStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public ChangeUserAccessStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result> Handle(ChangeUserAccessStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = await _context.CooperativeMembers.FirstOrDefaultAsync(a => a.CooperativeId == request.CooperativeId && a.UserId == request.UserId);
                // string message = "";
                if (appUser != null)
                {
                    appUser.CooperativeAccessStatus = request.CooperativeAccessStatus;
                    _context.CooperativeMembers.Update(appUser);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success(appUser);
                }
                return Result.Failure("User Not Found");
            }
            catch (Exception ex)
            {
               return Result.Failure(new string[] { "User status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
