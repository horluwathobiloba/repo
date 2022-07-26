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

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands
{
    public class AddRequestToJoinTrackerCommand:IRequest<Result>
    {
        public string Name { get; set; }
        public string UserEmail { get; set; }
      
        public string AdminEmail { get; set; }
        public int CooperativeId { get; set; }
        public RequestType RequestType { get; set; }
        public string LoggedInUserId { get; set; }
    }

    public class AddRequestToJoinTrackerCommandHandler : IRequestHandler<AddRequestToJoinTrackerCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;
        public AddRequestToJoinTrackerCommandHandler(IApplicationDbContext context, IIdentityService identityService, IAuthenticateService authenticateService)
        {
            _context = context;
            _identityService = identityService;
            _authenticateService = authenticateService;
        }
        public async Task<Result> Handle(AddRequestToJoinTrackerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var coop = await _context.Cooperatives.FirstOrDefaultAsync(x => x.Id == request.CooperativeId);
                if (coop is null)
                {
                    return Result.Failure("Coop does not exist");
                }
                var entity = new RequestToJoinTracker
                {
                    AdminEmail = request.AdminEmail,
                    Status = Status.Inactive,
                    StatusDesc = Status.Inactive.ToString(),
                    CooperativeId = request.CooperativeId,
                    UserEmail = request.UserEmail,
                    Name = request.Name,
                    CreatedDate = DateTime.Now,
                    CooperativeAccessStatus=CooperativeAccessStatus.Processing
                };
                await _context.RequestToJoinTrackers.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Request created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure("Request creation was not successful"+ ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
