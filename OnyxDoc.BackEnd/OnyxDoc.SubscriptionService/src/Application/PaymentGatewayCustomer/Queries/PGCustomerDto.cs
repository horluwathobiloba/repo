using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using OnyxDoc.SubscriptionService.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.PGCustomers.Queries
{
    public class PGCustomerDto : IMapFrom<PGCustomer>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int PaymentGatewayCustomerId { get; set; }
        public string PaymentGatewayCustomerCode { get; set; }
        public PaymentGateway PaymentGateway { get; set; }

        public SubscriberDto Subscriber { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PGCustomer, PGCustomerDto>();
            profile.CreateMap<PGCustomerDto, PGCustomer>();
        }
    }
}
