using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnyxDoc.AuthService.Application.Branding.Queries.GetBranding
{
    public class BrandingDto : IMapFrom<Domain.Entities.Branding>
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeColorCode { get; set; }
        public string FooterInformation { get; set; }
        public bool HasFooterInformation { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Branding, BrandingDto>();
            profile.CreateMap<BrandingDto, Domain.Entities.Branding>();
        }
    }
}
