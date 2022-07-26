using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnyxDoc.AuthService.Application.Common.Interfaces;
using OnyxDoc.AuthService.Application.Common.Models;
using OnyxDoc.AuthService.Domain.Entities;
using OnyxDoc.AuthService.Domain.Enums;
using OnyxDoc.AuthService.Infrastructure.Auth;
using OnyxDoc.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnyxDoc.AuthService.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<AuthToken> GenerateDeveloperToken(string userName, string Id)
        {

            try
            {
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, userName),
                new Claim (JwtRegisteredClaimNames.Sub, Id),
                //new Claim (ClaimType.Role, Customer.Role.Name),
            };
                JwtSecurityToken token = new TokenBuilder()
                .AddAudience(_configuration["Token:aud"])
                .AddIssuer(_configuration["Token:issuer"])
                .AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
                .AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
                .AddClaims(claims)
                .Build();
                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var newToken = new AuthToken()
                {
                    AccessToken = accessToken,
                    ExpiresIn = Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"])
                };
                return await Task.FromResult(newToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<AuthToken> GenerateAccessToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, user.Email),
                 new Claim ("role", user.Role?.Name),
                 new Claim ("roleaccesslevel", user.Role?.RoleAccessLevel.ToString()),
                 new Claim ("userid", user.UserId),
                 new Claim ("subscriberId", user.SubscriberId.ToString())
                };
                JwtSecurityToken token = new TokenBuilder()
               .AddAudience(_configuration["Token:aud"])
               .AddIssuer(_configuration["Token:issuer"])
               .AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
               .AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
               .AddClaims(claims)
               .Build();

                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var newToken = new AuthToken()
                {
                    AccessToken = accessToken,
                    ExpiresIn = Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]),
                };
                return await Task.FromResult(newToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> GenerateUserToken(string subscriberType, User user, object rolePermissions , Branding branding)
        {
            try
            {
                var permissions = JsonConvert.SerializeObject(rolePermissions);
                var userEntity = JsonConvert.SerializeObject(user);
                if (branding == null)
                {
                    branding = new Branding();
                }
                var brandingDetail = JsonConvert.SerializeObject(branding);
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, user.Email),
                 new Claim (JwtRegisteredClaimNames.Sub, user.Role.Name),
                 new Claim ("SubscriberType", subscriberType),
                new Claim ("UserEntity", userEntity),
                };
                if (rolePermissions != null )
                {
                    claims.Add(new Claim("RolePermissions", permissions));
                }
                if (branding != null)
                {
                    claims.Add(new Claim("Branding", brandingDetail));
                }
                JwtSecurityToken token = new TokenBuilder()
               .AddAudience(_configuration["Token:aud"])
               .AddIssuer(_configuration["Token:issuer"])
               .AddExpiry(Convert.ToInt32(_configuration["TokenConstants:ExpiryInMinutes"]))
               .AddKey(Encoding.UTF8.GetBytes(_configuration["TokenConstants:key"]))
               .AddClaims(claims)
               .Build();

                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);


                return await Task.FromResult(accessToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
