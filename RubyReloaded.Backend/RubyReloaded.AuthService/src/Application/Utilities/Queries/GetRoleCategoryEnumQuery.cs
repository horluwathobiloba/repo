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
    public class GetRoleCategoryEnumQuery:IRequest<Result>
    {

    }
    public class GetRoleCategoryEnumQueryHandler : IRequestHandler<GetRoleCategoryEnumQuery, Result>
    {
        public GetRoleCategoryEnumQueryHandler()
        {

        }
        public async Task<Result> Handle(GetRoleCategoryEnumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(RoleCategory));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                return Result.Failure("Operation was not successful :" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
