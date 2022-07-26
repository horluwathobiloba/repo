using AutoMapper;
using System;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Domain.Common;

namespace RubyReloaded.AuthService.Application.PermissionSets.Queries.GetPermissionSets
{
    public class PermissionSetDto : IMapFrom<PermissionSet>
    {
        public int Id { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string AccessLevelDesc { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public string StatusDesc { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PermissionSet, PermissionSetDto>();
            profile.CreateMap<PermissionSetDto, PermissionSet>();
        }
    }
}
