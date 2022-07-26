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
    public class GetAllAccessLevelsQuery:IRequest<Result>
    {
    }


    public class GetAllAccessLevelsQueryHandlers : IRequestHandler<GetAllAccessLevelsQuery, Result>
    {
        
        public async Task<Result> Handle(GetAllAccessLevelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetNames(typeof(AccessLevel));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Operation was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
