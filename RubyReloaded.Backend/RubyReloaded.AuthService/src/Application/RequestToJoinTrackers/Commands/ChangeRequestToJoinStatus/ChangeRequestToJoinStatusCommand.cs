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

namespace RubyReloaded.AuthService.Application.RequestToJoinTrackers.Commands.ChangeRequestToJoinStatus
{
    public class ChangeRequestToJoinStatusCommand:IRequest<Result>
    {
        public int RequestToJoinTrackerId { get; set; }
        public CooperativeAccessStatus cooperativeAccessStatus { get; set; }
        public string LoggedInUser { get; set; }
    }
    public class ChangeRequestToJoinStatusCommandHandler : IRequestHandler<ChangeRequestToJoinStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;

        public ChangeRequestToJoinStatusCommandHandler(IApplicationDbContext context, IIdentityService identityService, IAuthenticateService authenticateService)
        {
            _context = context;
            _identityService = identityService;
            _authenticateService = authenticateService;
        }

        public async Task<Result> Handle(ChangeRequestToJoinStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.RequestToJoinTrackers.FirstOrDefaultAsync(x => x.Id == request.RequestToJoinTrackerId);
                if (entity is null)
                {
                    return Result.Failure("Coop does not exist");
                }
                entity.CooperativeAccessStatus = request.cooperativeAccessStatus;
                _context.RequestToJoinTrackers.Update(entity);
                return Result.Success("Request created successfully", entity);
            }
            catch (Exception ex)
            {
                return Result.Failure("Request update was not successful" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
