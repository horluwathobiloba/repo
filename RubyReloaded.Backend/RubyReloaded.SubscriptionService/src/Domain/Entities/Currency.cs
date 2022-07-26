using RubyReloaded.SubscriptionService.Domain.Common;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubyReloaded.SubscriptionService.Domain.Entities
{
    public class Currency : AuditableEntity
    { 
        public CurrencyCode CurrencyCode { get; set; }
        public string CurrencyCodeDesc { get; set; }
        [DefaultValue(false)]
        public bool IsDefault { get; set; }   
    }
}
