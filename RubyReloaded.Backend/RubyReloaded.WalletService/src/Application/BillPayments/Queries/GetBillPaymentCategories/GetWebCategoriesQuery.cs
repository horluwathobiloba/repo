using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BillPayments.Queries
{
    public class GetWebCategoriesQuery : IRequest<Result>
    {
    }
    public class GetWebCategoriesQueryHandler : IRequestHandler<GetWebCategoriesQuery, Result>
    {

        private readonly IProvidusBankService _providus;
        public GetWebCategoriesQueryHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(GetWebCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Meant to check for 
                var categories = await _providus.GetBillPaymentCategories<List<ProvidusWebCategory>>();

                if (categories is null)
                {
                    return Result.Failure("Failed to retrive categories");
                }
                return Result.Success(categories);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting categories was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
