using Microsoft.EntityFrameworkCore;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Utility
{
    public class DomainCheckService : IDomainCheckService
    {
        public async Task<List<Subscriber>> DomainExists(IApplicationDbContext _context, SubscriberType subscriberType, string domain)
        {
            var subscribers = await _context.Subscribers.Where(a=>a.SubscriberType == subscriberType).ToListAsync();
            List<Subscriber> existingSubscribers = new List<Subscriber>();
            if (subscribers != null && subscribers.Count > 0)
            {
                var emailDomains = (EmailDomain[])Enum.GetValues(typeof(EmailDomain));
                foreach (var emailDomain in emailDomains)
                {
                    var domainString = emailDomain.ToString().ToLower();
                    var existingDomain = subscribers.FirstOrDefault(a => a.ContactEmail.ToLower().Contains(domain) && domain.Contains(domainString));
                    if (existingDomain != null)
                    {
                        existingSubscribers.Add(existingDomain);
                    }
                
                }
            }
            return existingSubscribers;
        }
    }
}
