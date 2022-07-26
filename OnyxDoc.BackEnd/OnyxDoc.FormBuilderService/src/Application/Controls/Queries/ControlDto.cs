using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.ControlProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using OnyxDoc.FormBuilderService.Application.Common.Models;

namespace OnyxDoc.FormBuilderService.Application.Controls.Queries
{
    public class ControlDto : BaseDTO, IMapFrom<Control>
    {
        public string DisplayName { get; set; }
        public string ControlTips { get; set; }
        public int InitialControlVersionId { get; set; }
        public decimal VersionNumber { get; set; }
        public InputValueType InputValueType { get; set; }
        public string InputValueTypeDesc { get; set; }
        public ControlType ControlType { get; set; }
        public string ControlTypeDesc { get; set; }
        public BlockControlType? BlockControlType { get; set; }
        public string BlockControlTypeDesc { get; set; }
        public FieldControlType? FieldControlType { get; set; }
        public string FieldControlTypeDesc { get; set; }

        public List<ControlPropertyDto> ControlProperties { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Control, ControlDto>();
            profile.CreateMap<ControlDto, Control>();

            profile.CreateMap<ControlProperty, ControlPropertyDto>();
            profile.CreateMap<ControlPropertyDto, ControlProperty>();
        }
    }
}
