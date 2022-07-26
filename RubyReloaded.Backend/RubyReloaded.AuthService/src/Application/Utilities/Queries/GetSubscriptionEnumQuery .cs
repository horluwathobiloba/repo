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
    public class GetSubscriptionEnumQuery :IRequest<Result>
    {

    }
    public class GetSubscriptionEnumQueryHandler : IRequestHandler<GetSubscriptionEnumQuery , Result>
    {
        
        public async Task<Result> Handle(GetSubscriptionEnumQuery  request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(SubscriptionType));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                return Result.Failure("Operation was not successful :" + ex?.Message ?? ex?.InnerException.Message);
            }
        }
    }
}
