using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums; 
using System.Collections.Generic; 
using System.Text;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries;

namespace OnyxDoc.FormBuilderService.Application.ControlProperties.Queries
{
    public class ControlPropertyDto : BaseDTO, IMapFrom<Control>
    {
     
        public string DisplayName { get; set; }
        public string PropertyTips { get; set; } 
        public decimal VersionNumber { get; set; }
        public ControlPropertyValueType ControlPropertyValueType { get; set; }
        public bool ShowInContextMenu { get; set; }
        public ControlPropertyType ControlPropertyType { get; set; }
        public string ControlPropertyTypeDesc { get; set; }

        public List<ControlPropertyItemDto> ControlPropertyItems { get; set; }     

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ControlProperty, ControlPropertyDto>();
            profile.CreateMap<ControlPropertyDto, ControlProperty>();

            profile.CreateMap<ControlPropertyItem, ControlPropertyItemDto>();
            profile.CreateMap<ControlPropertyItemDto, ControlPropertyItem>();
        }
    }
}
