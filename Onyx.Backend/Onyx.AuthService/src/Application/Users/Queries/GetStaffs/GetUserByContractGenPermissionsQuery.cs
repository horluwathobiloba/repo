using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Enums;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Onyx.AuthService.Domain.Entities;

namespace Onyx.AuthService.Application.Users.Queries.GetUsers
{
    public class GetUserByContractGenPermissionsQuery : IRequest<Result>
    {
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
    }

    public class GetUserByContractGenPermissionsQueryHandler : IRequestHandler<GetUserByContractGenPermissionsQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetUserByContractGenPermissionsQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Result> Handle(GetUserByContractGenPermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUserById(request.UserId);
                if (result.staff == null)
                {
                    return Result.Failure(new string[] { $"User does not exist with Id: {request.UserId}" });
                }
                //get users in the organization based on the id
                var resultStaffs = await _identityService.GetUsersByOrganizationId(request.OrganizationId);
                if (resultStaffs.staffs == null)
                {
                    return Result.Failure(new string[] { $"Please contact Admin to configures users for your organization" });
                }
                //get permissions for generate
                var permissionName = Regex.Replace(PowerUsersPermissions.GenerateContract.ToString(), "([A-Z])", " $1").Trim();
                var permissions = await _context.RolePermissions.Where(a=>a.Permission == permissionName && a.OrganizationId == request.OrganizationId).Distinct().ToListAsync();

                if (permissions == null)
                {
                    return Result.Failure(new string[] { $"Invalid Permissions specified" });
                }

                List<User> users = new List<User>();
                foreach (var permission in permissions)
                {
                    var userWithRole =  resultStaffs.staffs.Where(a => a.RoleId == permission.RoleId).ToList();
                    users.AddRange(userWithRole);
                }
                return Result.Success(users);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Error retrieving staff by Id: ", ex?.Message ?? ex?.InnerException?.Message });
            }
        }
    }
}
