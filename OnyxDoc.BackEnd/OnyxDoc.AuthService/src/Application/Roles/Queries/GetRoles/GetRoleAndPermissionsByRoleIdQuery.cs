using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using System.Collections.Generic;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Roles.Queries.GetRoles
{
    public class GetRoleAndPermissionsByRoleIdQuery : IRequest<Result>
    {
        public int  RoleId { get; set; }
        public int SubscriberId { get; set; }
        public string UserId { get; set; }

        public AccessLevel AccessLevel { get; set; }
    }

    public class GetRoleAndPermissionsByRoleIdQueryHandler : IRequestHandler<GetRoleAndPermissionsByRoleIdQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetRoleAndPermissionsByRoleIdQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetRoleAndPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var subscriber = await _context.Subscribers.Where(a => a.Id == request.SubscriberId).FirstOrDefaultAsync();
                if (subscriber == null)
                {
                    return Result.Failure("Invalid Subscriber Specified");
                }
                var userCheck = await _identityService.GetUserByIdAndSubscriber(request.UserId, request.SubscriberId);
                if (userCheck.user == null)
                {
                    return Result.Failure("User does not exist in this organisation");
                }
                var role = await _context.Roles.FirstOrDefaultAsync(a=>a.Id == request.RoleId);
                var roleValue = _mapper.Map<RoleDto>(role);
                var permissions = await _context.RolePermissions.Where(a=>a.RoleId == role.Id).ToListAsync();
                var category = permissions.Select(a=>a.Category).Distinct().ToList();
                //get all features based 
                var features = await _context.Features.Where(a => a.AccessLevel == request.AccessLevel && a.SubscriberId == request.SubscriberId).ToListAsync();
                if (features != null && features.Count > 0)
                {

                }

                var permissionsDictionary = new Dictionary<string, List<RolePermission>>();
                foreach (var item in category)
                {
                    permissionsDictionary.Add(item, permissions.Where(a => a.Category == item).ToList());
                }


                return Result.Success(new { Role = roleValue, permissionsDictionary });
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Retrieving roles and permissions by subscriber Id was not successful" + ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
