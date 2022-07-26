using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Onyx.AuthService.Application.Common.Interfaces;
using Onyx.AuthService.Application.Common.Models;
using Onyx.AuthService.Domain.Entities;
using Onyx.AuthService.Infrastructure.Auth;
using Onyx.AuthService.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.AuthService.Infrastructure.Authentication
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

        public async Task<AuthToken> GenerateAccessToken(User staff)
        {
            try
            {
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, staff.Email),
                 new Claim ("role", staff.Role.Name),
                 new Claim ("userid", staff.UserId),
                 new Claim ("organizationId", staff.OrganizationId.ToString())
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

        public async Task<string> GenerateUserToken(User staff, List<RolePermission> rolePermissions)
        {
            try
            {
                var permissions = JsonConvert.SerializeObject(rolePermissions);
                var staffEntity = JsonConvert.SerializeObject(staff);
                List<Claim> claims = new List<Claim>() {
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim (JwtRegisteredClaimNames.Email, staff.Email),
                 new Claim (JwtRegisteredClaimNames.Sub, staff.Role.Name),
                 new Claim (JwtRegisteredClaimNames.Sub, staff.Role.Name),
                new Claim ("UserEntity", staffEntity),
                };
                if (rolePermissions != null && rolePermissions.Count > 0)
                {
                    claims.Add(new Claim("RolePermissions", permissions));
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

        public async Task<LoginDetails> DecodeRefreshToken(string refreshToken)
        {
            try
            {

                var handler = new JwtSecurityTokenHandler();
                var decodedValue = handler.ReadJwtToken(refreshToken);
                //var token = new JwtSecurityToken(refreshToken);

                //return login details
                var tokenParam = new LoginDetails()
                {
                    UserName=decodedValue.Claims.First(Claim =>Claim.Type == "userName").Value,
                    Password = decodedValue.Claims.First(Claim => Claim.Type == "hash").Value
                };

                return await Task.FromResult(tokenParam);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<RefreshToken> GenerateRefreshToken(string userName, string password)
        {
            try
            {

                List<Claim> myClaims = new List<Claim>() {
                //new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim ("userName", userName),
                 new Claim ("hash", password)};

                JwtSecurityToken token = new TokenBuilder()
               .AddExpiry(Convert.ToInt32(_configuration["RefreshTokenConstants:ExpiryInMinutes"]))
               .AddClaims(myClaims)
               .Build();
                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var newToken = new RefreshToken()
                {
                    RefreshAccessToken = accessToken,
                    ExpiresIn = Convert.ToInt32(_configuration["RefreshTokenConstants:ExpiryInMinutes"])
                };


                return await Task.FromResult(newToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
