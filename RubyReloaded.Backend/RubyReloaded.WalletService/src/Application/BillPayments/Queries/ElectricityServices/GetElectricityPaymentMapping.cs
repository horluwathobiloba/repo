using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BillPayments.Queries.ElectricityServices
{
    public class GetElectricityPaymentMapping : IRequest<Result>
    {
        public int CategoryId { get; set; }
    }

    public class GetElectricityPaymentMappingHandler : IRequestHandler<GetElectricityPaymentMapping, Result>
    {
        private readonly IProvidusBankService _providus;
        public GetElectricityPaymentMappingHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(GetElectricityPaymentMapping request, CancellationToken cancellationToken)
        {

            try
            {
             //   var mapping = await _providus.GetElectricityServicePayment<GetAirtimeMapping>(request.CategoryId);
                //if (mapping is null)
                //{
                //    return Result.Failure("Mapping could not be found");
                //}
                //return Result.Success(mapping.fields);
                return Result.Success("");
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Getting mapping was not successful", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
