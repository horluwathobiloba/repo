using AutoMapper;
using RubyReloaded.AuthService.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace RubyReloaded.AuthService.Application.Users.Queries.GetUser
{
    public class UserDto: IMapFrom<Domain.Entities.User>
    {

        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }





        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.User, UserDto>();
            profile.CreateMap<UserDto, Domain.Entities.User>();
        }
    }
}
