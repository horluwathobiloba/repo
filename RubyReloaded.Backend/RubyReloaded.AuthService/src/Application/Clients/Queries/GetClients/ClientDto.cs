using AutoMapper;
using RubyReloaded.AuthService.Application.Common.Mappings;
using RubyReloaded.AuthService.Domain.Entities;
using System;
using RubyReloaded.AuthService.Domain.Enums;

namespace RubyReloaded.AuthService.Application.Clients.Queries.GetClients
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
