using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetClients
{
    public class ClientListDto : IMapFrom<Client>
{
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Client, ClientListDto > ();
            profile.CreateMap<ClientListDto, Client>();
        }
    }
}
