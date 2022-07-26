using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Queries.GetSuperAdminUsers
{
    public class GetSystemOwnerUserByEmail:IRequest<Result>
    {
        public string Email { get; set; }
        public int UserAccessLevel { get; set; }
    }

    public class GetSystemOwnerUserByEmailHandler : IRequestHandler<GetSystemOwnerUserByEmail, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        public GetSystemOwnerUserByEmailHandler(IIdentityService identityService, IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }
        public async Task<Result> Handle(GetSystemOwnerUserByEmail request, CancellationToken cancellationToken)
        {
            try
            {
                var level = Convert.ToInt32(UserAccessLevel.SystemOwner);
                if (request.UserAccessLevel!=level)
                {
                    return Result.Failure("Invalid Credentials");
                }
                var result = await _identityService.GetUserByEmail(request.Email);
                var sytemOwnerUser = await _context.SystemOwnerUsers.FirstOrDefaultAsync(x => x.Email == request.Email);
                var entity = new
                {
                    firstname = result.user.FirstName,
                    lastname = result.user.LastName,
                    roleid = sytemOwnerUser.RoleId,
                    email = sytemOwnerUser.Email,
                  
                };
                return Result.Success(entity);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
