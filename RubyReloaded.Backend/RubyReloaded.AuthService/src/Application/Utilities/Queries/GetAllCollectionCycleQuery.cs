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
    public class GetAllCollectionCycleQuery:IRequest<Result>
    {
    }

    public class GetAllCollectionCycleQueryHandlers : IRequestHandler<GetAllCollectionCycleQuery, Result>
    {

        public async Task<Result> Handle(GetAllCollectionCycleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(CollectionCycle));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
