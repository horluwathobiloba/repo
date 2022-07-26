using RubyReloaded.WalletService.Domain.ViewModels;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IPaystackService
    {
        Task<PaystackPaymentResponse> GetTransactionStatus(PaymentIntentVm paymentIntentVm);
        Task<PaystackPaymentResponse> InitializeTransaction(PaymentIntentVm paymentIntentVm);
        Task<PaystackPaymentResponse> ChargeAuthorization(PaymentIntentVm paymentIntentVm);
        Task<PaystackPaymentResponse> DeactivateAuthorization(string authorizationCode);
    }
}
