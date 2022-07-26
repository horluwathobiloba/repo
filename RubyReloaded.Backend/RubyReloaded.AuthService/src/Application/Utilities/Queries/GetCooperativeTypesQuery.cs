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
    public class GetCooperativeTypesQuery:IRequest<Result>
    {
    }

    public class GetCooperativeTypesQueryHandlers : IRequestHandler<GetCooperativeTypesQuery, Result>
    {

        public async Task<Result> Handle(GetCooperativeTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(CooperativeType));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
