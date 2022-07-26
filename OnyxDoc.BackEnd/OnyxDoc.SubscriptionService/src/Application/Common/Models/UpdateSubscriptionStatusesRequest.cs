using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OnyxDoc.SubscriptionService.Application.Common.Models
{
    public class UpdateSubscriptionStatusesRequest
    {
        public int SubscriberId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int Id { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; } 
        public SubscriberDto Subscriber { get; set; }

        public Status Status
        {
            get
            {
                var status = Status.Inactive;
                switch (this.SubscriptionStatus)
                {
                    case SubscriptionStatus.Active:
                        status = Status.Active;
                        break;
                    case SubscriptionStatus.Cancelled:
                        status = Status.Deactivated;
                        break;
                    case SubscriptionStatus.Expired:
                        status = Status.Deactivated;
                        break;
                    case SubscriptionStatus.FreeTrial:
                        status = Status.Active;
                        break;
                    case SubscriptionStatus.ProcessingPayment:
                        status = Status.Inactive;
                        break;
                    default:
                        break;
                }
                return status;
            }
        }
    }
}