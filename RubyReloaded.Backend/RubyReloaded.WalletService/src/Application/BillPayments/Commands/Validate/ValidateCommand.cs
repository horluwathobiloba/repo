using MediatR;
using RubyReloaded.WalletService.Application.Common.Interfaces;
using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.BillPayments.Commands.Validate
{
    public class ValidateCommand : IRequest<Result>
    {
        public List<Input> Inputs { get; set; }
        public int BillId { get; set; }
        public string UserId { get; set; }
    }


    public class ValidateCommandHandler : IRequestHandler<ValidateCommand, Result>
    {
        private readonly IProvidusBankService _providus;
        public ValidateCommandHandler(IProvidusBankService providus)
        {
            _providus = providus;
        }
        public async Task<Result> Handle(ValidateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Meant to check for 
                var validateRequest = new ProvidusValidationRequest
                {
                    inputs = request.Inputs
                };
                var response = await _providus.Validate(validateRequest, request.BillId);
                if (response is null)
                {
                    return Result.Failure("Validation Failed");
                }
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure(new string[] { "Validation Failed", ex?.Message ?? ex?.InnerException.Message });
            }
        }
    }
}
