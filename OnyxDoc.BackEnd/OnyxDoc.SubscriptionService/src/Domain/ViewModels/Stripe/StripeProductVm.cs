using OnyxDoc.SubscriptionService.Domain.Common;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnyxDoc.SubscriptionService.Domain.ViewModels
{
    public class StripeProductVm
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [NotMapped]
        public StripeAccountVm Plan { get; set; }
    }
}
