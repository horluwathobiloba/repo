using AutoMapper;
using OnyxDoc.FormBuilderService.Application.Common.Mappings;
using OnyxDoc.FormBuilderService.Application.PageControlItems.Queries;
using OnyxDoc.FormBuilderService.Domain.Entities;
using OnyxDoc.FormBuilderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnyxDoc.FormBuilderService.Application.DocumentPages.Queries
{
    public class DocumentPageDto : IMapFrom<DocumentPage>
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public int DocumentId { get; set; }
        public int PageIndex { get; set; }
        public int PageNumber { get; set; }
        public PageLayout PageLayout { get; set; }


        #region Page Dimension
        public string Height { get; set; }
        public string Width { get; set; }
        public string Position { get; set; }
        public string Transform { get; set; }
        #endregion

        public string HeaderData { get; set; }
        public string FooterData { get; set; }
 
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; } 

        public List<PageControlItemDto> PageControlItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentPage, DocumentPageDto>();
            profile.CreateMap<DocumentPageDto, DocumentPage>();

            profile.CreateMap<PageControlItem, PageControlItemDto>();
            profile.CreateMap<PageControlItemDto, PageControlItem>();
        }
    }
}
