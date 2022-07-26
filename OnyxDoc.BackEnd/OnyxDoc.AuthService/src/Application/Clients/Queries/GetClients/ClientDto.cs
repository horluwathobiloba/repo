using AutoMapper;
using OnyxDoc.AuthService.Application.Common.Mappings;
using OnyxDoc.AuthService.Domain.Entities;
using System;
using OnyxDoc.AuthService.Domain.Enums;

namespace OnyxDoc.AuthService.Application.Clients.Queries.GetClients
{
    public class ClientDto : IMapFrom<Client>
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Status Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Client, ClientDto>();
            profile.CreateMap<ClientDto, Client>();
        }
    }
}
