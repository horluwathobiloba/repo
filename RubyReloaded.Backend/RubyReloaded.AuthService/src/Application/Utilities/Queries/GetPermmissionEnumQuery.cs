using MediatR;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Application.Utilities.Queries
{
    public class GetPermmissionEnumQuery:IRequest<Result>
    {

    }
    public class GetPermmissionEnumQueryHandler : IRequestHandler<GetPermmissionEnumQuery, Result>
    {
      
        public async Task<Result> Handle(GetPermmissionEnumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var supportEnums = Enum.GetValues(typeof(SupportPermissions));
                var externalUserEnums = Enum.GetValues(typeof(ExternalUserPermissions));
                var powerUserEnums = Enum.GetValues(typeof(PowerUsersPermissions));
                var adminUserEnums = Enum.GetValues(typeof(AdminPermissions));
                var superAdminUserEnums = Enum.GetValues(typeof(SuperAdminPermissions));
                var testEnums = Enum.GetValues(typeof(TestPermissions));
                var result = new
                {
                    superAdminUserEnums,
                    externalUserEnums,
                    powerUserEnums,
                    adminUserEnums,
                    supportEnums,
                    testEnums
                };
                return Result.Success(result);
            }
            catch (Exception ex)
            {

                return Result.Failure("Operation was not successful :" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
