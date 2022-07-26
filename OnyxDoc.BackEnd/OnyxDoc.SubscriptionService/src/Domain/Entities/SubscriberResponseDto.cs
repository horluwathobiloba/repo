using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Domain.Entities
{
    public class SubscriberResponseDto
    {
            public SubscriberAdminDto user { get; set; }
            public List<RoleObjectDto> role { get; set; }
            public SubscriberObjectDto subscriberResponse { get; set; }
        
    }
}
