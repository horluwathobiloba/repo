using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.FormBuilderService.Application.Common.Interfaces
{
    public interface ISubscriberService
    {
        public SubscriberDto Subscriber { get; set; }
        public SubscriberObjectDto SubscriberObject { get; set; }
        public SubscriberAdminDto SubscriberAdmin { get; set; }

        public List<SubscriberDto> Subscribers { get; set; }
        public string AuthToken { get; set; }
        Task<EntityVm<SubscriberResponseDto>> SignUpSubscriberAsync(string authToken, CreateSubscriberRequest request);
        Task<bool> ActivateSubscriberFreeTrialAsync(string authToken, int subscriberId,  string userId);
        Task<bool> CompleteSubscriberFreeTrialAsync(string authToken, int subscriberId, string userId);

    }
}
