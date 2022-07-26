using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using RubyReloaded.AuthService.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RubyReloaded.AuthService.Domain.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RubyReloaded.AuthService.Application.PermissionSets.Commands.CreatePermissionSet
{
    public class CreatePermissionSetCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
    }

    public class CreatePermissionSetCommandHandler : IRequestHandler<CreatePermissionSetCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly IIdentityService _identityService;

        public CreatePermissionSetCommandHandler(IApplicationDbContext context, IIdentityService identityService, IAuthenticateService authenticateService)
        {
            _context = context;
            _identityService = identityService;
            _authenticateService = authenticateService;
        }

        public async Task<Result> Handle(CreatePermissionSetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _identityService.GetUserById(request.UserId);
                if (user.user == null)
                {
                    return Result.Failure(new string[] { "Unable to create feature.Invalid User ID credentials!" });
                }
                if (user.user==null)
                {
                    return Result.Failure("User does not exist");
                }
                var feature = new PermissionSet
                {
                    Name = request.Name,
                    ParentID = request.ParentID,
                    ParentName = request.ParentName,
                    CreatedBy =  user.user.Email,
                    CreatedById = request.UserId,
                    CreatedDate = DateTime.Now,
                    AccessLevel = request.AccessLevel,
                    AccessLevelDesc = request.AccessLevel.ToString(),
                    Status = Status.Active,
                    StatusDesc = Status.Active.ToString()
                };

                await _context.PermissionSets.AddAsync(feature);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success("PermissionSet created successfully", feature);
            }
            catch (Exception ex)
            {

                return Result.Failure(new string[] { "PermissionSet creation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }

        }


    }
 }



