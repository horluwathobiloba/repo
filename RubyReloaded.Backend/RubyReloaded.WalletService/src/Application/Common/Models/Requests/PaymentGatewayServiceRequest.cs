using RubyReloaded.WalletService.Domain.Enums;

namespace RubyReloaded.WalletService.Application.Common.Models.Requests
{
    public class PaymentGatewayServiceRequest
    {
        public Status Status { get; set; }

        public PaymentGatewayServiceCategory PaymentGatewayServiceCategory { get; set; }
    }
}
