using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.Common.Models;
using OnyxDoc.FormBuilderService.Application.Controls.Queries;
using OnyxDoc.FormBuilderService.Application.PageControlItemProperties.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.PageControlItems.Queries
{
    public class PageControlItemDto : BaseDTO, IMapFrom<PageControlItem>
    {
        public string SubscriberName { get; set; }
        public int DocumentPageId { get; set; }
        public int ControlId { get; set; }

        #region Value captured based on the control's input value type
        public string TextValue { get; set; }
        public long NumberValue { get; set; }
        public decimal FloatValue { get; set; }
        public bool BooleanValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public string BlobValue { get; set; }


        /// <summary>
        /// Get the value based on the control value type
        /// </summary> 
        public object GetValue
        {
            get
            {
                switch (this.Control?.InputValueType)
                {
                    case InputValueType.Boolean:
                        return BooleanValue;

                    case InputValueType.Date:
                        return DateTimeValue;

                    case InputValueType.Number:
                        return NumberValue;

                    case InputValueType.String:
                        return TextValue;

                    case InputValueType.Image:
                    case InputValueType.Signature:
                    case InputValueType.File:
                        return BlobValue;

                    default:
                        return null;
                }
            }
        }
        #endregion

        public PageControlItemDto PageControlItem { get; set; }
        public ControlDto Control { get; set; }

        #region Page ControlItem Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public List<PageControlItemPropertyDto> PageControlItemProperties { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PageControlItem, PageControlItemDto>();
            profile.CreateMap<PageControlItemDto, PageControlItem>();

            profile.CreateMap<PageControlItemProperty, PageControlItemPropertyDto>();
            profile.CreateMap<PageControlItemPropertyDto, PageControlItemProperty>();

            profile.CreateMap<PageControlItem, PageControlItemDto>();
            profile.CreateMap<PageControlItemDto, PageControlItem>();

            profile.CreateMap<Control, ControlDto>();
            profile.CreateMap<ControlDto, Control>();
        }
    }
}
