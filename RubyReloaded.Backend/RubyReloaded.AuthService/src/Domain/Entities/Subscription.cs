using RubyReloaded.AuthService.Domain.Common;
using RubyReloaded.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Domain.Entities
{
    public class Subscription:AuditableEntity
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string SuscriberCode { get; set; }
        public string Location { get; set; }
        public decimal SubscriptionAmount { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string Currency { get; set; }
        //public string UserId { get; set; }
    }
}
