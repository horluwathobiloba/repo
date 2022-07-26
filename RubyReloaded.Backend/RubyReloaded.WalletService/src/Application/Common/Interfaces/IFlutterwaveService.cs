using RubyReloaded.WalletService.Domain.ViewModels;
using System.Threading.Tasks;

namespace RubyReloaded.WalletService.Application.Common.Interfaces
{
    public interface IFlutterwaveService 
    {
        Task<FlutterwaveResponse> InitiatePayment(FlutterwaveRequest request, string paymentHash);
        Task<FlutterwaveResponse> GetPaymentStatus(string transactionId);
    }
}
