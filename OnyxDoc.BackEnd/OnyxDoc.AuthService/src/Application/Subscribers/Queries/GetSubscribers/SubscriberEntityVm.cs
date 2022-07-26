using AutoMapper;
using System;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Domain.Common;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Subscribers.Queries.GetSubscribers
{
    public class SubscriberEntityVm : IMapFrom<Subscriber>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public string SubscriberCode { get; set; }
        public string Logo { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Subscriber, SubscriberDto>();
            profile.CreateMap<SubscriberDto, Subscriber>();

            profile.CreateMap<Subscriber, SubscriberEntityVm>();
            profile.CreateMap<SubscriberEntityVm, Subscriber>();
        }
    }
}
