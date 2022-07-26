using Microsoft.Extensions.Configuration;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Utility
{
    public class GenerateUserInviteLinkService : IGenerateUserInviteLinkService
    {
        private readonly IStringHashingService _stringHashingService;
        private readonly IConfiguration _configuration;
        public GenerateUserInviteLinkService(IStringHashingService stringHashingService)
        {
            _stringHashingService = stringHashingService;
        }

        public async Task<UserInviteLinkVm> ConfirmUserLink(string inviteLink)
        {

            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(inviteLink);
            var subscriber=decodedValue.Claims.First(Claim => Claim.Type == "subscriberId").Value;
            var role = decodedValue.Claims.First(Claim => Claim.Type == "RoleId").Value;

            var userDetails = new UserInviteLinkVm()
            {
                UserId = decodedValue.Claims.First(Claim => Claim.Type == "UserId").Value,
                RecipientEmail = decodedValue.Claims.First(Claim => Claim.Type == "RecipientEmail").Value,
                SubscriberId = Convert.ToInt32(subscriber),
                RoleId = Convert.ToInt32(role)
            };
            return userDetails;
        }

        public async Task<string> GenerateHashLink(UserHashCode hashCode)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim ("email", hashCode.UserEmail),
                new Claim ("subscriberId", hashCode.SubscriberId.ToString()),
                new Claim("DateTime", DateTime.Now.Ticks.ToString())
            };
            JwtSecurityToken link = new TokenBuilder()
               //.AddAudience(_configuration["Token:aud"])
               //.AddIssuer(_configuration["Token:issuer"])
               //.AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
               //.AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
               .AddClaims(claims)
               .Build();

            string accessToken = new JwtSecurityTokenHandler().WriteToken(link);
            return accessToken;
        }

        public async Task<string> GenerateUserInviteLink(UserInviteLink inviteLink)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim ("RoleId", inviteLink.RoleId.ToString()),
                new Claim ("UserId", inviteLink.UserId),
                new Claim ("subscriberId", inviteLink.SubscriberId.ToString()),
                new Claim("RecipientEmail", inviteLink.RecipientEmail),
                new Claim("DateTime", DateTime.Now.Ticks.ToString())
            };
            JwtSecurityToken link = new TokenBuilder()
               //.AddAudience(_configuration["Token:aud"])
               //.AddIssuer(_configuration["Token:issuer"])
               //.AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
               //.AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
               .AddClaims(claims)
               .Build();

            string accessToken = new JwtSecurityTokenHandler().WriteToken(link);
            return accessToken;
        }

        public async Task<string> GenerateUserLink(UserInviteLink inviteLink)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim ("RoleId", inviteLink.RoleId.ToString()),
                new Claim ("UserId", inviteLink.UserId),
                new Claim ("subscriberId", inviteLink.SubscriberId.ToString()),
                //new Claim("RecipientEmail", inviteLink.RecipientEmail)
            };
            JwtSecurityToken link = new TokenBuilder()
               //.AddAudience(_configuration["Token:aud"])
               //.AddIssuer(_configuration["Token:issuer"])
               //.AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
               //.AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
               .AddClaims(claims)
               .Build();

            string accessToken = new JwtSecurityTokenHandler().WriteToken(link);
            return accessToken;
        }
    }
}
