using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace OnyxDoc.AuthService.Application.Features.Commands.ChangeFeature
{
    public class ChangeFeatureStatusCommand :  IRequest<Result>
    {
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public int FeatureId { get; set; }
    }

    public class ChangeFeatureCommandHandler : IRequestHandler<ChangeFeatureStatusCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ChangeFeatureCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(ChangeFeatureStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Invalid User" });
                }
                var org = await _context.Subscribers.FirstOrDefaultAsync(a => a.Id == user.user.SubscriberId);
                if (org == null)
                {
                    return Result.Failure(new string[] { "Unable to change feature status. User does not belong to Subscriber" });
                }
                string message = "";
                var feature = await _context.Features.FirstOrDefaultAsync(a=>a.Id == request.FeatureId);
                if (feature == null)
                {
                    return Result.Failure(new string[] { "Invalid Feature" });
                }
                switch (feature.Status)
                {
                    case Domain.Enums.Status.Active:
                        feature.Status = Domain.Enums.Status.Inactive;
                        message = "Feature deactivation was successful";
                        break;
                    case Domain.Enums.Status.Inactive:
                        feature.Status = Domain.Enums.Status.Active;
                        message = "Feature activation was successful";
                        break;
                    case Domain.Enums.Status.Deactivated:
                        feature.Status = Domain.Enums.Status.Active;
                        message = "Feature activation was successful";
                        break;
                    default:
                        break;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(message);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Feature status change was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
