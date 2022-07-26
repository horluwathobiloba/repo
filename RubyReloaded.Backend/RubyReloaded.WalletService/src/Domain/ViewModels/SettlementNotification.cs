using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.ViewModels
{
    public class SettlementNotification
    {
        public string SessionId { get; set; }
        public string AccountNumber { get; set; }
        public string TranRemarks { get; set; }
        public string TransactionAmount { get; set; }
        public string SettledAmount { get; set; }
        public string FeeAmount { get; set; }
        public string VATAmount { get; set; }
        public string Currency { get; set; }
        public string InitiationTranRef { get; set; }
        public string SettlementId { get; set; }
        public string SourceAccountNumber { get; set; }
        public string SourceAccountName { get; set; }
        public string SourceBankName { get; set; }
        public string ChannelId { get; set; }
        public string TranDateTime { get; set; }
    }
}
