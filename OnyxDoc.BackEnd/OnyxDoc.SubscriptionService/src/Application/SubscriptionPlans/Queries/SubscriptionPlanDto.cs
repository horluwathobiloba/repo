using AutoMapper;
using OnyxDoc.SubscriptionService.Application.Common.Mappings;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanPricings.Queries;
using OnyxDoc.SubscriptionService.Application.SubscriptionPlanFeatures.Queries;
using OnyxDoc.SubscriptionService.Domain.Entities;
using OnyxDoc.SubscriptionService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.SubscriptionService.Application.SubscriptionPlans.Queries
{
    public class SubscriptionPlanDto : IMapFrom<SubscriptionPlan>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string SubscriptionTypeDesc { get; set; }       
        public int NumberOfUsers { get; set; }         
        public int NumberOfTemplates { get; set; } 
        public int StorageSize { get; set; } 
        public StorageSizeType StorageSizeType { get; set; }
        public string StorageSizeTypeDesc { get; set; }
        public bool AllowMonthlyPricing { get; set; }
        public bool AllowYearlyPricing { get; set; }
        public string Period { get; set; } 
        public bool AllowFreeTrial { get; set; } 
        public bool AllowDiscount { get; set; } 
        public bool ShowSubscribeButton { get; set; } 
        public bool ShowContactUsButton { get; set; } 
        public decimal Discount { get; set; }
        public int FreeTrialPeriod { get; set; }
        public bool IsHighlighted { get; set; }
        public FreeTrialPeriodFrequency FreeTrialPeriodFrequency { get; set; }
        public List<SubscriptionPlanPricingDto> SubscriptionPlanPricings { get; set; }
        public List<SubscriptionPlanFeatureDto> SubscriptionPlanFeatures { get; set; }

        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
            profile.CreateMap<SubscriptionPlanDto, SubscriptionPlan>();

            profile.CreateMap<SubscriptionPlanPricing, SubscriptionPlanPricingDto>();
            profile.CreateMap<SubscriptionPlanPricingDto, SubscriptionPlanPricing>();

            profile.CreateMap<SubscriptionPlanFeature, SubscriptionPlanFeatureDto>();
            profile.CreateMap<SubscriptionPlanFeatureDto, SubscriptionPlanFeature>();
        }
    }
}
