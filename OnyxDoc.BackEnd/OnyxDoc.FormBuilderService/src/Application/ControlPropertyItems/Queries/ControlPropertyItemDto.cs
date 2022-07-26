using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums; 
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.ControlPropertyItems.Queries
{
    public class ControlPropertyItemDto : BaseDTO, IMapFrom<Control>
    {
        public int ControlPropertyId { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public bool IsDefaultValue { get; set; }

        public void Mapping(Profile profile)
        { 
            profile.CreateMap<ControlPropertyItem, ControlPropertyItemDto>();
            profile.CreateMap<ControlPropertyItemDto, ControlPropertyItem>(); 
        }
    }
}
