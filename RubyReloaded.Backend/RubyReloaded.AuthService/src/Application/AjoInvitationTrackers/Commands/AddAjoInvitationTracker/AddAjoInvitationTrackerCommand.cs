using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.AjoInvitationTrackers.Commands.AddAjoInvitationTracker
{
    public class AddAjoInvitationTrackerCommand:IRequest<Result>
    {
        public string UserEmail { get; set; }
        public string AdminEmail { get; set; }
        public int AjoId { get; set; }
        public RequestType RequestType { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class AddAjoInvitationTrackerCommandHandler : IRequestHandler<AddAjoInvitationTrackerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;
        public AddAjoInvitationTrackerCommandHandler(IApplicationDbContext context, IIdentityService identityService, IAuthenticateService authenticateService)
        {
            _context = context;
            _identityService = identityService;
            _authenticateService = authenticateService;
        }
        public async Task<Result> Handle(AddAjoInvitationTrackerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ajo = await _context.Ajos.FirstOrDefaultAsync(x => x.Id == request.AjoId);
                if (ajo is null)
                {
                    return Result.Failure("Coop does not exist");
                }
                var entity = new AjoInvitationTracker
                {
                    AdminEmail = request.AdminEmail,
                    Status = Status.Inactive,
                    StatusDesc = Status.Inactive.ToString(),
                    AjoId = request.AjoId,
                    UserEmail = request.UserEmail,
                    CreatedDate = DateTime.Now,
                    //CooperativeAccessStatus = CooperativeAccessStatus.Processing
                };
                await _context.AjoInvitationTrackers.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Request created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure("Request creation was not successful" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
