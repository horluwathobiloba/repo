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
    public class GetRequestTypeEnumQuery:IRequest<Result>
    {

    }
    public class GetRequestTypeEnumQueryHandler : IRequestHandler<GetRequestTypeEnumQuery, Result>
    {
        public GetRequestTypeEnumQueryHandler()
        {

        }
        public async Task<Result> Handle(GetRequestTypeEnumQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(RequestType));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                return Result.Failure("Operation was not successful :" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
