using AutoMapper;
using RubyReloaded.SubscriptionService.Application.Common.Mappings;
using RubyReloaded.SubscriptionService.Domain.Entities;
using RubyReloaded.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.SubscriptionService.Application.SubscriptionPlanFeatures.Queries
{
    public class SubscriptionPlanFeatureDto : IMapFrom<SubscriptionPlanFeature>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public int ParentFeatureName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SubscriptionPlanFeature, SubscriptionPlanFeatureDto>();
            profile.CreateMap<SubscriptionPlanFeatureDto, SubscriptionPlanFeature>();
        }
    }
}
