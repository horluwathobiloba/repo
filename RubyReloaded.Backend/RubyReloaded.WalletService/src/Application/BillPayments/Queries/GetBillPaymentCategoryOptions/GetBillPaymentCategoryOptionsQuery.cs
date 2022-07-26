using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BillPayments.Queries.GetBillPaymentCategoryOptions
{
    public class GetBillPaymentCategoryOptionsQuery : IRequest<Result>
    {
        public int CategoryId { get; set; }
    }
    public class GetBillPaymentCategoryOptionsQueryHandler : IRequestHandler<GetBillPaymentCategoryOptionsQuery, Result>
    {
        private readonly IProvidusBankService _providus;
        public GetBillPaymentCategoryOptionsQueryHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(GetBillPaymentCategoryOptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //Meant to check for 
                var categories = await _providus.GetBillPaymentCategoryOptions<List<GetBillPaymentCategoryOptionsResponse>>(request.CategoryId);

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
