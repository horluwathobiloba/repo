using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Airtime.Queries
{
    public class GetAirtimeCategory:IRequest<Result>
    {

    }
    public class GetAirtimeCategoryHandler : IRequestHandler<GetAirtimeCategory, Result>
    {
        private readonly IProvidusBankService _providus;
        public GetAirtimeCategoryHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(GetAirtimeCategory request, CancellationToken cancellationToken)
        {
            try
            {
                //Meant to check for 
                var categories = await _providus.GetAirtimeCategories();
             
                if (categories is null)
                {
                    return Result.Failure("Failed to retrive airtime categories");
                }
                return Result.Success(categories);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting airtime categories was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
