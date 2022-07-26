using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries
{
    public class SubscriptionPlanFeatureDto : IMapFrom<SubscriptionPlanFeature>
    {
        public int Id { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ParentFeatureId { get; set; }
        public string ParentFeatureName { get; set; }
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
