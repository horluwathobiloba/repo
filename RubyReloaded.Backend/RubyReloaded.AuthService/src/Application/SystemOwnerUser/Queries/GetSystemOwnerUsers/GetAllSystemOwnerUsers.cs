using MediatR;
using Microsoft.EntityFrameworkCore;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.SuperAdmin.Queries.GetSuperAdminUsers
{
    public class GetAllSystemOwnerUsers:IRequest<Result>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetAllSystemOwnerUsersHandler : IRequestHandler<GetAllSystemOwnerUsers, Result>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;

        public GetAllSystemOwnerUsersHandler(IIdentityService identityService,IApplicationDbContext context)
        {
            _identityService = identityService;
            _context = context;
        }
        public async Task<Result> Handle(GetAllSystemOwnerUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.SystemOwnerUsers.Skip(request.Skip).Take(request.Take).ToListAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
