using Newtonsoft.Json;
using RubyReloaded.WalletService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.WalletService.Domain.Entities
{
    public class CardAuthorization : AuditableEntity
    {
            public string AuthorizationCode { get; set; }
            public string Bin { get; set; }
            public string LastDigits { get; set; }
            public string ExpiryMonth { get; set; }
            public string ExpiryYear { get; set; }
            public string Channel { get; set; }
            public string CardType { get; set; }
            public string Bank { get; set; }
            public string CountryCode { get; set; }
            public string Brand { get; set; }
            public string CurrencyCode { get; set; }
            public string Hash { get; set; }
            public string Signature { get; set; }
            public string AccountName { get; set; }
            public string ReferenceId { get; set; }
            [JsonIgnore]
            public decimal Amount { get; set; }
            public string UserId { get; set; }

    }
}
