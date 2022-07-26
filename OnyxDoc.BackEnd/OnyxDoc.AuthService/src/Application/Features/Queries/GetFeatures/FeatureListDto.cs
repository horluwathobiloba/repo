
using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Common;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Features.Queries.GetFeatures
{
    public class FeatureListDto :  IMapFrom<Feature>
{
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool IsDefault { get; set; }
        public bool ShouldShowOnNavBar { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Feature, FeatureListDto > ();
            profile.CreateMap<FeatureListDto, Feature>();
        }
    }
}
