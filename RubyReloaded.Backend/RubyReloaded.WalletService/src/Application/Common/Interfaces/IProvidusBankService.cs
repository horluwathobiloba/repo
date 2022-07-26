using RubyReloaded.WalletService.Application.Common.Models;
using RubyReloaded.WalletService.Application.Common.Models.Requests;
using RubyReloaded.WalletService.Application.Common.Models.Response;
using RubyReloaded.WalletService.Application.PaymentIntegrations.Queries.GetProvidusBanks;
using RubyReloaded.WalletService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IProvidusBankService:IBankService
    {
        Task<ProvidusMakePaymentResponse> MakePayment(ProvidusMakePaymentRequest providusMakePaymentRequest);
        Task<ProvidusValidationResponse> Validate(ProvidusValidationRequest providusValidationRequest,int billId);
        Task<ProvidusFundTransferResponse> ProvidusFundTransfer(ProvidusFundTransferRequest providusFundTransferRequest);
        Task<DynamicAccountNumberResponseDto> CreateDynamicAccountNumber(string requestObject);
        Task<NipFundTransferResponse> NipFundTransfer(NipFundTransferRequest nipFundTransferRequest);
        Task<GetProvidusAccountResponse> GetWalletBalance(GetProvidusAccountRequest getProvidusAccountRequest);
        Task<GetAllProvidusBankResponse> GetProvidusBanks();

    }
}
