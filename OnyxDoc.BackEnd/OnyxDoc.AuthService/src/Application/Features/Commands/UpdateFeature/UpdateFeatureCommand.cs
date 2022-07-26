using OnyxDoc.AuthService.Application.Common.Exceptions;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace OnyxDoc.AuthService.Application.Features.Commands.UpdateFeature
{
    public partial class UpdateFeatureCommand :  IRequest<Result>
    {
        public int FeatureId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public bool ShouldShowOnNavBar { get; set; }

    }

    public class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateFeatureCommandHandler(IApplicationDbContext context,IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }
        

        public async Task<Result> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to update feature.Invalid User ID and Subscriber credentials!" });
                }
                var getFeatureForUpdate = await _context.Features.FirstOrDefaultAsync(a=>a.Id == request.FeatureId);
                if (getFeatureForUpdate == null)
                {
                    return Result.Failure(new string[] { "Invalid Feature" });
                }
                getFeatureForUpdate.Name = request.Name.Trim();
                getFeatureForUpdate.ParentID = request.ParentID;
                getFeatureForUpdate.ParentName = request.ParentName;
                getFeatureForUpdate.LastModifiedById = request.UserId;
                getFeatureForUpdate.LastModifiedDate = DateTime.Now;
                getFeatureForUpdate.AccessLevel = request.AccessLevel;
                getFeatureForUpdate.AccessLevelDesc = request.AccessLevel.ToString();
                getFeatureForUpdate.SubscriberId = request.SubscriberId;
                getFeatureForUpdate.ShouldShowOnNavBar = request.ShouldShowOnNavBar;

                _context.Features.Update(getFeatureForUpdate);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success("Feature updated successfully", getFeatureForUpdate);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Error updating feature: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
