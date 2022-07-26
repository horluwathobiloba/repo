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

namespace OnyxDoc.AuthService.Application.Features.Commands.CreateFeature
{
    public class CreateFeatureCommand : IRequest<Result>
    {
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

    public class CreateFeatureCommandHandler : IRequestHandler<CreateFeatureCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;

        public CreateFeatureCommandHandler(IApplicationDbContext context, IIdentityService identityService, IAuthenticateService authenticateService)
        {
            _context = context;
            _identityService = identityService;
            _authenticateService = authenticateService;
        }

        public async Task<Result> Handle(CreateFeatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to create feature.Invalid User ID and Subscriber credentials!" });
                }
                if (user.user==null)
                {
                    return Result.Failure("User does not exist");
                }
                var feature = new Feature
                {
                    Name = request.Name,
                    ParentID = request.ParentID,
                    ParentName = request.ParentName,
                    CreatedByEmail =  user.user.Email,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    SubscriberId = request.SubscriberId,
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString(),
                    ShouldShowOnNavBar = request.ShouldShowOnNavBar,
                };

                await _context.Features.AddAsync(feature);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("Feature created successfully", feature);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "Feature creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }


    }
 }



