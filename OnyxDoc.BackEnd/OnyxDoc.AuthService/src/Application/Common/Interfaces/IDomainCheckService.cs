using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Application.Common.Interfaces
{
   public interface IDomainCheckService
    {
        Task<List<Subscriber>> DomainExists(IApplicationDbContext applicationDbContext , SubscriberType subscriberType,string domain);
    }
}
