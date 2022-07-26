using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BillPayments.Queries.ElectricityServices
{
    public class GetElectricityServiceCategoriesQuery : IRequest<Result>
    {

    }
    public class GetElectricityServiceCategoriesQueryHandler : IRequestHandler<GetElectricityServiceCategoriesQuery, Result>
    {
        private readonly IProvidusBankService _providus;
        public GetElectricityServiceCategoriesQueryHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(GetElectricityServiceCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Meant to check for 
                var categories = await _providus.GetElectricityServiceCategories();

                if (categories is null)
                {
                    return Result.Failure("Failed to retrive categories");
                }
                return Result.Success(categories);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] {"Getting categories was not successful", ex?.Message ?? ex?.InnerException.Message});
            }
        }
    }
}
