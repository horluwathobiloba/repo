using RubyReloaded.SubscriptionService.Application.Common.Interfaces;
using System;

namespace RubyReloaded.SubscriptionService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
