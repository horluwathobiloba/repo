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
    public class GetCurrencyCodesQuery : IRequest<Result>
    {
    }

    public class GetCurrencyCodesQueryHandlers : IRequestHandler<GetCurrencyCodesQuery, Result>
    {

        public async Task<Result> Handle(GetCurrencyCodesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var enums = Enum.GetValues(typeof(CurrencyCode));
                return Result.Success(enums);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
