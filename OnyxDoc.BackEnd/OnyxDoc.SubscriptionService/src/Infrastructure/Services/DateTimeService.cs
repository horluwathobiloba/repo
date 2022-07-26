using OnyxDoc.SubscriptionService.Application.Common.Interfaces;
using System;

namespace OnyxDoc.SubscriptionService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
