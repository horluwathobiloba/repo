using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using System.Collections.Generic;

namespace OnyxDoc.FormBuilderService.Application.PageControlItemPropertyValues.Queries
{
    public class PageControlItemPropertyValueDto : BaseDTO, IMapFrom<PageControlItemPropertyValue>
    { 
        public int PageControlItemPropertyId { get; set; }
        public string PropertyValue { get; set; }

        public List<PageControlItemPropertyDto> PageControlItemProperties { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap< PageControlItemPropertyValue, PageControlItemPropertyValueDto>();
            profile.CreateMap<PageControlItemPropertyValueDto, PageControlItemPropertyValue>();

            profile.CreateMap<PageControlItemProperty, PageControlItemPropertyDto>();
            profile.CreateMap<PageControlItemPropertyDto, PageControlItemProperty>();
        }
    }
}
