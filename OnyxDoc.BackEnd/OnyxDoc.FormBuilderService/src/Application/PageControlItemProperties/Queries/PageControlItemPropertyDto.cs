using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries
{
    public class PageControlItemPropertyDto : IMapFrom<PageControlItemProperty>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int SubscriptionPlanId { get; set; }
        public int PageControlItemId { get; set; }
        public int ControlPropertyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public PageControlItemDto PageControlItem { get; set; }

        public ControlPropertyDto ControlProperty { get; set; }

        public List<PageControlItemPropertyValueDto> PageControlItemPropertyValues { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PageControlItemProperty, PageControlItemPropertyDto>();
            profile.CreateMap<PageControlItemPropertyDto, PageControlItemProperty>();

            profile.CreateMap<PageControlItemPropertyValue, PageControlItemPropertyValueDto>();
            profile.CreateMap<PageControlItemPropertyValueDto, PageControlItemPropertyValue>();

            profile.CreateMap<PageControlItem, PageControlItemDto>();
            profile.CreateMap<PageControlItemDto, PageControlItem>();

            profile.CreateMap<ControlProperty, ControlPropertyDto>();
            profile.CreateMap<ControlPropertyDto, ControlProperty>();
        }
    }
}
