using OnyxDoc.SubscriptionService.Application.Common.Models;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.SubscriptionService.Application.Common.Interfaces
{
    public interface ISubscriberService
    {
        public SubscriberDto Subscriber { get; set; }
        public SubscriberObjectDto SubscriberObject { get; set; }
        public SubscriberAdminDto SubscriberAdmin { get; set; }

        public List<SubscriberDto> Subscribers { get; set; }
        public string AuthToken { get; set; }
        Task<EntityVm<SubscriberResponseDto>> SignUpSubscriberAsync(string authToken, CreateSubscriberRequest request);
        Task<EntityVm<SubscriberDto>> ActivateSubscriberFreeTrialAsync(string authToken, int subscriberId,  string userId);
        Task<bool> CompleteSubscriberFreeTrialAsync(string authToken, int subscriberId, string userId);

    }
}
