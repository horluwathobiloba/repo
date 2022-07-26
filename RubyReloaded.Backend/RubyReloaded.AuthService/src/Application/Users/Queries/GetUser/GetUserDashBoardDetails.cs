using MediatR;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Users.Queries.GetUser
{
    public class GetUserDashBoardDetails:IRequest<Result>
    {

    }
    public class GetUserDashBoardDetailsHandler : IRequestHandler<GetUserDashBoardDetails, Result>
    {
        private readonly IIdentityService _identityService;
        public GetUserDashBoardDetailsHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<Result> Handle(GetUserDashBoardDetails request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.GetUsersDashboardAsync();
                return result;
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "DashBoard Retrieval was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
