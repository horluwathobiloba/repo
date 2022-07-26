using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RubyReloaded.AuthService.Application.Common.Interfaces;
using RubyReloaded.AuthService.Application.Common.Models;
using RubyReloaded.AuthService.Domain.Entities;
using RubyReloaded.AuthService.Domain.Enums;
using RubyReloaded.AuthService.Infrastructure.Auth;
using RubyReloaded.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RubyReloaded.AuthService.Infrastructure.Authentication
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
                //new Claim (ClaimType.Role, User.Role.Name),
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

        public async Task<AuthToken> GenerateAccessToken(User staff)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, staff.Email),
                 new Claim ("userid", staff.UserId),
                 new Claim ("bvn", staff.BVN is null ?"":staff.BVN),
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

        public async Task<AuthToken> GenerateInviteToken(string email,string code,string entityId,LinkType type)
        {
            try
            {
                int val = (int)type;
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti,code ),
                new Claim (JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Sub,entityId),
                new Claim("linktype",val.ToString())
                };
                JwtSecurityToken token = new TokenBuilder()
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



        public async Task<string> GenerateUserToken(User staff)
        {
            try
            {
                var staffEntity = JsonConvert.SerializeObject(staff);
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, staff.Email),
                new Claim ("UserEntity", staffEntity),
                };
               
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
